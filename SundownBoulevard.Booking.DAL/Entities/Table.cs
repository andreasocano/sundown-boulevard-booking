using System;
using System.ComponentModel.DataAnnotations;

namespace SundownBoulevard.Booking.DAL.Entities
{
    public class Table
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
        public int Seats { get; set; }
        public DateTimeOffset CreatedDate { get; set; }
    }
}
