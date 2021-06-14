using System.Collections.Generic;

namespace SundownBoulevard.Booking.DAL.Entities
{
    public class Order
    {
        public int ID { get; set; }
        public int ReservationID { get; set; }
        public virtual Reservation Reservation { get; set; }
        public ICollection<OrderLine> OrderLines { get; set; }
    }
}
