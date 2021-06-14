using SundownBoulevard.Booking.DAL.Entities;
using SundownBoulevard.Booking.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SundownBoulevard.Booking.API.Services
{
    public class HasSufficientAmountOfSeatsService
    {
        private readonly ReservationRepository _reservationRepository;

        public HasSufficientAmountOfSeatsService(ReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;
        }

        /// <summary>
        /// Checks whether a list of tables have enough seats for a specified reservation.
        /// </summary>
        /// <param name="tables"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        public bool HasSufficientAmountOfSeats(IEnumerable<Table> tables, Guid uid)
        {
            // Check if they make up enough seats to finish reservation
            var reservation = _reservationRepository.Get(uid);
            if (reservation == null) return false;
            return tables.Sum(t => t.Seats) >= reservation.Seats;
        }
    }
}
