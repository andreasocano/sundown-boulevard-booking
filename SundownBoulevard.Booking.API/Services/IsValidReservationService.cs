using Microsoft.Extensions.Configuration;
using SundownBoulevard.Booking.DAL.Entities;
using SundownBoulevard.Booking.DAL.Repositories;
using System;

namespace SundownBoulevard.Booking.API.Services
{
    public class IsValidReservationService
    {
        private readonly ReservationRepository _reservationRepository;
        private readonly int _minutesToExpiry;
        public IsValidReservationService(ReservationRepository reservationRepository, IConfiguration configuration)
        {
            _reservationRepository = reservationRepository;
            if (!int.TryParse(configuration["ReservationMinutesToExpiry"], out _minutesToExpiry))
                throw new InvalidCastException("Unable to cast expiration time from configuration.");
        }

        /// <summary>
        /// Validates a reservation based on when the reservation was activated and a specified activation code.
        /// </summary>
        /// <param name="uid"></param>
        /// <param name="activationCode"></param>
        /// <returns></returns>
        public bool IsValidReservation(Guid uid, string activationCode)
        {
            var reservation = _reservationRepository.Get(uid);
            if (!IsValid(reservation)) return false;
            if (string.IsNullOrWhiteSpace(activationCode)) return false;
            if (string.IsNullOrWhiteSpace(reservation.ActivationCode)) return false;
            return activationCode.Equals(reservation.ActivationCode);
        }

        /// <summary>
        /// Validates a reservation based on when the reservation was activated.
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public bool IsValid(Reservation reservation)
        {
            if (reservation == null) return false;
            if (reservation.Seats <= 0) return false;
            var minutesSinceActivation = (DateTime.Now - reservation.ReservedDate).TotalMinutes;
            return minutesSinceActivation < _minutesToExpiry;
        }
    }
}
