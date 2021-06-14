using SundownBoulevard.Booking.DAL.Entities;
using System;
using System.Collections.Generic;

namespace SundownBoulevard.Booking.API.Models
{
    public class TableSchedule
    {
        public Table Table { get; set; }
        public IEnumerable<TimeSpan> ReservationStartTimes { get; set; } = new List<TimeSpan>();
    }
}
