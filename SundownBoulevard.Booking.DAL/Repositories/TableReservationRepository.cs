using SundownBoulevard.Booking.DAL.Entities;
using SundownBoulevard.Booking.DAL.Enums;
using System;
using System.Collections.Generic;

namespace SundownBoulevard.Booking.DAL.Repositories
{
    public class TableReservationRepository
    {
        private readonly RestaurantContext _restaurantContext;
        private readonly ReservationRepository _reservationRepository;

        public TableReservationRepository(ReservationRepository reservationRepository, RestaurantContext restaurantContext)
        {
            _reservationRepository = reservationRepository;
            _restaurantContext = restaurantContext;
        }

        public DataOperationResult Save(IEnumerable<Table> tables, Guid uid)
        {
            var reservation = _reservationRepository.Get(uid);
            if (reservation == null) return DataOperationResult.Failure;
            var seats = reservation.Seats;
            foreach (var table in tables)
            {
                if (seats <= 0) break;
                Save(table, reservation);
                seats -= table.Seats;
            }
            return DataOperationResult.Success;
        }

        public void Save(Table table, Reservation reservation)
        {
            _restaurantContext.TableReservations.Add(new TableReservation
            {
                TableID = table.ID,
                ReservationID = reservation.ID
            });
            _restaurantContext.SaveChanges();
        }
    }
}
