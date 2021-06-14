using SundownBoulevard.Booking.DAL.Entities;
using SundownBoulevard.Booking.DAL.Enums;
using System;

namespace SundownBoulevard.Booking.DAL.Repositories
{
    public class OrderRepository
    {
        private readonly RestaurantContext _restaurantContext;
        private readonly ReservationRepository _reservationRepository;

        public OrderRepository(RestaurantContext restaurantContext, ReservationRepository reservationRepository)
        {
            _restaurantContext = restaurantContext;
            _reservationRepository = reservationRepository;
        }

        public DataOperationResult Create(Guid reservationID, string drink, string dish)
        {
            var reservation = _reservationRepository.Get(reservationID);
            if (reservation == null) return DataOperationResult.Failure;
            var order = _restaurantContext.Orders.Add(new Order
            {
                ReservationID = reservation.ID
            });
            _restaurantContext.SaveChanges();

            var drinkLine = _restaurantContext.OrderLines.Add(new OrderLine { Item = drink,  OrderID = order.Entity.ID });
            var dishLine = _restaurantContext.OrderLines.Add(new OrderLine { Item = dish, OrderID = order.Entity.ID });
            _restaurantContext.SaveChanges();

           

            order.Entity.OrderLines.Add(drinkLine.Entity);
            order.Entity.OrderLines.Add(dishLine.Entity);
            _restaurantContext.SaveChanges();
            return DataOperationResult.Success;
        }
    }
}
