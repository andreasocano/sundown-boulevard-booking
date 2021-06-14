using Microsoft.Extensions.Configuration;
using SundownBoulevard.Booking.DAL.Repositories;
using SundownBoulevard.Booking.DTO.Models;
using System;

namespace SundownBoulevard.Booking.API.Services
{
    public class ActivateReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly SendEmailService _sendEmailService;
        private readonly string _confirmingBookingSubject;
        private readonly string _confirmingBookingBody;
        public ActivateReservationService(ReservationRepository reservationRepository, SendEmailService sendEmailService, IConfiguration configuration)
        {
            _reservationRepository = reservationRepository;
            _sendEmailService = sendEmailService;
            _confirmingBookingSubject = configuration["ConfirmingBookingSubject"];
            _confirmingBookingBody = configuration["ConfirmingBookingBody"];
        }

        /// <summary>
        /// Activates a reservation by updating it's state and sending an email to enable confirming / booking it.
        /// </summary>
        /// <param name="request"></param>
        public async void ActivateReservation(ActivateReservationRequest request)
        {
            var activationCode = Guid.NewGuid().ToString();
            _reservationRepository.SaveActivatedState(request.UID, request.Email, activationCode);
            await _sendEmailService.SendEmail(new[] { request.Email }, _confirmingBookingSubject, string.Format(_confirmingBookingBody, activationCode));
        }
    }
}
