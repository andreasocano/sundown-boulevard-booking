using System;

namespace SundownBoulevard.Booking.DTO.Models
{
    public class ActivateReservationRequest
    {
        public Guid UID { get; set; }
        public string Email { get; set; }
    }
}
