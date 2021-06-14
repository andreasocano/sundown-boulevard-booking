using System;
using System.ComponentModel.DataAnnotations;

namespace SundownBoulevard.Booking.DAL.Entities
{
    public class Booking
    {
        [Key]
        public int ID { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
        public int ReservationID { get; set; }
        public virtual Reservation Reservation { get; set; }
    }
}
