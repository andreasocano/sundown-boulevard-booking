namespace SundownBoulevard.Booking.API.Models
{
    public class BookingConfiguration
    {
        public int AllocatedHoursForBooking { get; set; }
        public int EarliestBookingHour { get; set; }
        public int LatestBookingHour { get; set; }
        public int TimeIncrements { get; set; }
    }
}
