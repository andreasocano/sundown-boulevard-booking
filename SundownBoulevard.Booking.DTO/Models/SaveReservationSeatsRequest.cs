using System;

namespace SundownBoulevard.Booking.DTO.Models
{
    public class SaveReservationSeatsRequest
    {
        public Guid UID { get; set; }
        public int Amount { get; set; }
    }
}
