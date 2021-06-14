using Newtonsoft.Json;
using System.Collections.Generic;

namespace SundownBoulevard.Booking.Website.Models
{
    public class DishWrapper
    {
        [JsonProperty("meals")]
        public List<Dish> Dishes { get; set; }
    }
}