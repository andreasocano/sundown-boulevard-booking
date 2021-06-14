using System.ComponentModel.DataAnnotations;

namespace SundownBoulevard.Booking.DAL.Entities
{
    public class TableReservation
    {
        [Key]
        public int ID { get; set; }
        public int TableID { get; set; }
        public int ReservationID { get; set; }
    }
}
