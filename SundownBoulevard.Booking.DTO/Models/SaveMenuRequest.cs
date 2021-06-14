using System;

namespace SundownBoulevard.Booking.DTO.Models
{
    public class SaveMenuRequest
    {
        public Guid UID { get; set; }
        public string Drink { get; set; }
        public string Dish { get; set; }
    }
}
