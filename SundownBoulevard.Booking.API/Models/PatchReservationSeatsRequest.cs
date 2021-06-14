using System;

namespace SundownBoulevard.Booking.API.Models
{
    public class PatchReservationSeatsRequest
    {
        public int Amount { get; set; }
        public Guid UID { get; set; }
    }
}
