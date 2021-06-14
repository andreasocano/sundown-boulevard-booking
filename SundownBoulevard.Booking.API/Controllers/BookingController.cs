using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SundownBoulevard.Booking.API.Models;
using SundownBoulevard.Booking.API.Services;

namespace SundownBoulevard.Booking.API.Controllers
{
    [EnableCors("AllowWebsite")]
    [ApiController]
    [Route("[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly IsValidReservationService _isValidReservationService;
        private readonly FinalizeBookingService _finalizeBookingService;

        public BookingController(ILogger<ReservationController> logger, IsValidReservationService isValidReservationService, FinalizeBookingService finalizeBookingService)
        {
            _logger = logger;
            _isValidReservationService = isValidReservationService;
            _finalizeBookingService = finalizeBookingService;
        }

        [HttpPost("confirm")]
        public IActionResult Confirm(ConfirmBookingRequest request)
        {
            if (!_isValidReservationService.IsValidReservation(request.UID, request.ActivationCode))
            {
                _logger.LogWarning($"Invalid reservation with UID: {request.UID}");
                return BadRequest("Reservation has expired.");
            }
            _finalizeBookingService.FinalizeBooking(request.UID);
            return Ok($"Reservation is valid.");
        }

    }
}
