using Microsoft.Extensions.Logging;
using SundownBoulevard.Booking.DAL.Entities;
using SundownBoulevard.Booking.DAL.Enums;
using System;
using System.Linq;

namespace SundownBoulevard.Booking.DAL.Repositories
{
    public class ReservationRepository
    {
        private readonly RestaurantContext _restaurantContext;
        private readonly ILogger<ReservationRepository> _logger;
        public ReservationRepository(RestaurantContext restaurantContext, ILogger<ReservationRepository> logger)
        {
            _restaurantContext = restaurantContext;
            _logger = logger;
        }

        public Guid Create()
        {
            var uid = Guid.NewGuid();
            _restaurantContext.Reservations.Add(new Reservation
            {
                UID = uid,
                CreatedDate = DateTime.Now
            });
            _restaurantContext.SaveChanges();
            return uid;
        }

        public Reservation Get(Guid uid)
        {
            var reservation = _restaurantContext.Reservations.FirstOrDefault(res => res.UID.Equals(uid));
            if (reservation == null) _logger.LogWarning($"Reservation requested with the following UID could not be found: {uid}");
            return reservation;
        }

        public DataOperationResult SaveSeats(Guid uid, int seats)
        {
            Reservation reservation = Get(uid);
            if (reservation == null) return DataOperationResult.Failure;
            reservation.Seats = seats;
            _restaurantContext.SaveChanges();
            return DataOperationResult.Success;
        }



        public DataOperationResult SaveDate(Guid uid, DateTime date)
        {
            Reservation reservation = Get(uid);
            if (reservation == null) return DataOperationResult.Failure;
            reservation.Date = date;
            reservation.ReservedDate = DateTime.Now;
            _restaurantContext.SaveChanges();
            return DataOperationResult.Success;
        }

        public DataOperationResult SaveActivatedState(Guid uid, string email, string activationCode)
        {
            Reservation reservation = Get(uid);
            if (reservation == null) return DataOperationResult.Failure;
            reservation.Email = email;
            reservation.ActivationCode = activationCode;
            reservation.ReservedDate = DateTime.Now;
            _restaurantContext.SaveChanges();
            return DataOperationResult.Success;
        }

        public Reservation Get(string activationCode)
        {
            var reservation = _restaurantContext.Reservations.FirstOrDefault(res => res.ActivationCode != null && res.ActivationCode.Equals(activationCode));
            if (reservation == null) _logger.LogWarning($"Reservation requested with the following activation code could not be found: {activationCode}");
            return reservation;
        }

        public string GetEmail(Guid uid)
        {
            var reservation = _restaurantContext.Reservations.FirstOrDefault(res => res.UID.Equals(uid));
            if (reservation == null) _logger.LogWarning($"Reservation requested with the following UID could not be found: {uid}");
            return reservation.Email;
        }

        public bool DatesMatch(Reservation one, Reservation other)
        {
            return one.Date.Year == other.Date.Year && one.Date.Month == other.Date.Month && one.Date.Day == other.Date.Day;
        }

        public bool TimesMatch(Reservation one, Reservation other)
        {
            return one.Date.TimeOfDay.Equals(other.Date.TimeOfDay);
        }
    }
}
