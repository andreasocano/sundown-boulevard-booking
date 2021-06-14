using System;

namespace SundownBoulevard.Booking.API.Models
{
    public class GetTimeSlotsRequest
    {
        public Guid UID { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
    }
}
