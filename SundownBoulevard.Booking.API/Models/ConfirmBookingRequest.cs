using System;

namespace SundownBoulevard.Booking.API.Models
{
    public class ConfirmBookingRequest
    {
        public Guid UID { get; set; }
        public string ActivationCode { get; set; }
    }
}
