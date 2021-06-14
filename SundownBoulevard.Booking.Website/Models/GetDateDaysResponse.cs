using System.Collections.Generic;

namespace SundownBoulevard.Booking.Website.Models
{
    public class GetDateDaysResponse
    {
        public ICollection<GetDateDaysItem> Items { get; set; } = new List<GetDateDaysItem>();
    }
}