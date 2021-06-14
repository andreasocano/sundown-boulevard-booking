using Microsoft.Extensions.Logging;
using SundownBoulevard.Booking.DAL.Entities;
using System;
using System.Linq;

namespace SundownBoulevard.Booking.DAL.Repositories
{
    public class BookingRepository
    {
        private readonly RestaurantContext _restaurantContext;
        private readonly ILogger<ReservationRepository> _logger;
        private readonly ReservationRepository _reservationRepository;
        public BookingRepository(RestaurantContext restaurantContext, ILogger<ReservationRepository> logger, ReservationRepository reservationRepository)
        {
            _restaurantContext = restaurantContext;
            _logger = logger;
            _reservationRepository = reservationRepository;
        }

        public void Create(Guid reservationID)
        {
            var reservation = _reservationRepository.Get(reservationID);
            if (reservation == null) _logger.LogWarning($"Booking could not be created for reservation ID: {reservationID}");
            var booking = _restaurantContext.Bookings.Add(new Entities.Booking
            {
                ReservationID = reservation.ID,
                CreatedDate = DateTime.Now
            });
            _restaurantContext.SaveChanges();
            booking.Entity.Reservation = reservation;
            _restaurantContext.SaveChanges();
        }

        public Entities.Booking Get(Guid uid)
        {
            var booking = _restaurantContext.Bookings.FirstOrDefault(b => b.Reservation.UID.Equals(uid));
            if (booking == null) _logger.LogWarning($"Booking requested with the following reservation ID could not be found: {uid}");
            return booking;
        }
    }
}
