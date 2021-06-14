using Microsoft.Extensions.Configuration;
using SundownBoulevard.Booking.DAL.Repositories;
using System;

namespace SundownBoulevard.Booking.API.Services
{
    public class FinalizeBookingService
    {
        private readonly BookingRepository _bookingRepository;
        private readonly SendEmailService _sendEmailService;
        private readonly string _confirmedBookingSubject;
        private readonly string _confirmedBookingBody;
        public FinalizeBookingService(BookingRepository bookingRepository, IConfiguration configuration, SendEmailService sendEmailService)
        {
            _bookingRepository = bookingRepository;
            _confirmedBookingSubject = configuration["ConfirmedBookingSubject"];
            _confirmedBookingBody = configuration["ConfirmedBookingBody"];
            _sendEmailService = sendEmailService;
        }

        /// <summary>
        /// Finalizes a booking, by creating a record of it and sending a confirmation email to the owner of that booking.
        /// </summary>
        /// <param name="uid"></param>
        public async void FinalizeBooking(Guid uid)
        {
            _bookingRepository.Create(uid);
            var booking = _bookingRepository.Get(uid);
            await _sendEmailService.SendEmail(new[] { booking.Reservation.Email }, _confirmedBookingSubject, string.Format(_confirmedBookingBody, booking.Reservation.Seats, booking.Reservation.Date.ToString("f")));
        }
    }
}
