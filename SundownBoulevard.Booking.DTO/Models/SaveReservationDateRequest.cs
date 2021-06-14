using System;

namespace SundownBoulevard.Booking.DTO.Models
{
    public class SaveReservationDateRequest
    {
        public Guid UID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public string TimeSlotStart { get; set; }
    }
}
