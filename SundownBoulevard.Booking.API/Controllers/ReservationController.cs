using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SundownBoulevard.Booking.API.Factories;
using SundownBoulevard.Booking.API.Models;
using SundownBoulevard.Booking.API.Repositories;
using SundownBoulevard.Booking.API.Services;
using SundownBoulevard.Booking.DAL.Enums;
using SundownBoulevard.Booking.DAL.Repositories;
using SundownBoulevard.Booking.DTO.Models;
using System;
using System.Linq;

namespace SundownBoulevard.Booking.API.Controllers
{
    [EnableCors("AllowWebsite")]
    [ApiController]
    [Route("[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly ReservationRepository _reservationRepository;
        private readonly ActivateReservationService _activateReservationService;
        private readonly TimeSlotFactory _timeSlotFactory;
        private readonly TableRepository _tableRepository;
        private readonly HasSufficientAmountOfSeatsService _hasSufficientAmountOfSeatsService;
        private readonly TableReservationRepository _tableReservationRepository;
        private readonly OrderRepository _orderRepository;

        public ReservationController(ILogger<ReservationController> logger, ReservationRepository reservationRepository, ActivateReservationService activateReservationService, TimeSlotFactory timeSlotFactory, TableRepository tableRepository, HasSufficientAmountOfSeatsService hasSufficientAmountOfSeatsService, TableReservationRepository tableReservationRepository, OrderRepository orderRepository)
        {
            _logger = logger;
            _reservationRepository = reservationRepository;
            _activateReservationService = activateReservationService;
            _timeSlotFactory = timeSlotFactory;
            _tableRepository = tableRepository;
            _hasSufficientAmountOfSeatsService = hasSufficientAmountOfSeatsService;
            _tableReservationRepository = tableReservationRepository;
            _orderRepository = orderRepository;
        }

        [HttpGet("initialize")]
        public IActionResult Initialize()
        {
            var uid = _reservationRepository.Create();
            return Ok(uid);
        }

        [HttpPost("seats")]
        public IActionResult Seats(SaveReservationSeatsRequest request)
        {
            var result = _reservationRepository.SaveSeats(request.UID, request.Amount);
            if (result == DataOperationResult.Failure)
            {
                _logger.LogWarning($"Could not save seats for UID: {request.UID}");
                return BadRequest("Unable to save seats.");
            }
            return Ok($"Received request to save {request.Amount} seats.");
        }

        [HttpPost("date")]
        public IActionResult Date(SaveReservationDateRequest request)
        {
            if (!TimeSpan.TryParse(request.TimeSlotStart, out var timeSlotStart))
            {
                var message = "Invalid time slot for start of reservation.";
                _logger.LogWarning($"{message} UID: {request.UID}");
                return BadRequest();
            }
            var date = new DateTime(request.Year, request.Month, request.Day, timeSlotStart.Hours, timeSlotStart.Minutes, 0);
            var tables = _tableRepository.GetAvailable(request.UID, date);
            if (!_hasSufficientAmountOfSeatsService.HasSufficientAmountOfSeats(tables, request.UID))
            {
                _logger.LogWarning($"Insufficient seats for UID: {request.UID}");
                return BadRequest("There's no longer a sufficient amount of avilable seats for reservation.");
            }
            var tableReservationsResult = _tableReservationRepository.Save(tables, request.UID);
            if (tableReservationsResult == DataOperationResult.Failure)
            {
                _logger.LogError($"Table reservations not created: {request.UID}");
                return BadRequest("Unable to perform table reservation.");
            }

            var result = _reservationRepository.SaveDate(request.UID, date);
            if (result == DataOperationResult.Failure)
            {
                var message = "Date is not available.";
                _logger.LogWarning($"{message} UID: {request.UID}");
                return BadRequest(message);
            }
            return Ok($"Received request to save date.");
        }

        [HttpPost("email")]
        public IActionResult Email(ActivateReservationRequest request)
        {
            var activationCode = Guid.NewGuid().ToString();
            _reservationRepository.SaveActivatedState(request.UID, request.Email, activationCode);
            return Ok($"Received request to save email '{request.Email}'.");
        }

        [HttpPost("activate")]
        public IActionResult Activate(ActivateReservationRequest request)
        {
            _activateReservationService.ActivateReservation(request);
            return Ok($"Received request to save email '{request.Email}'.");
        }

        [HttpGet("email/{uid}")]
        public IActionResult Email(Guid uid)
        {
            var email = _reservationRepository.GetEmail(uid);
            if (string.IsNullOrWhiteSpace(email))
            {
                var message = "Could not get email.";
                _logger.LogWarning($"{message} UID: {uid}");
                return BadRequest();
            }
            return Ok(email);
        }

        [HttpPost("time-slots/available")]
        public IActionResult TimeSlots(GetTimeSlotsRequest request)
        {
            var timeSlots = _timeSlotFactory.CreateAvailable(request);
            return Ok(timeSlots.Select(ts => ts.StartTime.ToString()));
        }

        [HttpGet("time-slots")]
        public IActionResult TimeSlots()
        {
            var timeSlots = _timeSlotFactory.GetAllTimeSlotStartTimes();
            return Ok(timeSlots.Select(ts => ts.ToString()));
        }

        [HttpPost("menu")]
        public IActionResult Menu(SaveMenuRequest request)
        {
            _orderRepository.Create(request.UID, request.Drink, request.Dish);
            return Ok("Saved menu.");
        }
    }
}
