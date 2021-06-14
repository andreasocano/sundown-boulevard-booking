using System;
using System.ComponentModel.DataAnnotations;

namespace SundownBoulevard.Booking.DAL.Entities
{
    public class Reservation
    {
        [Key]
        public int ID { get; set; }
        [Required]
        public Guid UID { get; set; }
        public int Seats { get; set; }
        public DateTime Date { get; set; }
        public string Email { get; set; }
        public string ActivationCode { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ReservedDate { get; set; }
    }
}
