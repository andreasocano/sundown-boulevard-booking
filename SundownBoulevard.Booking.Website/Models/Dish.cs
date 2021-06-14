using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SundownBoulevard.Booking.Website.Models
{
    public class Dish
    {
        [JsonProperty("strMeal")]
        public string Name { get; set; }
        [JsonProperty("strCategory")]
        public string Category { get; set; }
        [JsonProperty("strMealThumb")]
        public string ThumnailURL { get; set; }
        [JsonIgnore]
        public string Object { get; set; }
    }
}