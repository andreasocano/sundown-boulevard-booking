using System;
using System.Collections.Generic;

namespace SundownBoulevard.Booking.Website.Models
{
    public class DateViewModel : ReservationViewModel
    {
        public DateViewModel(string reservationID) : base(reservationID)
        {
        }

        public DateTime ReferenceDate { get; set; }
        public bool IsForCurrentYear { get; set; } = true;
        public IEnumerable<TimeSpan> AllTimeSlots { get; set; }
        public IEnumerable<TimeSpan> SpecificTimeSlots { get; set; }
    }
}