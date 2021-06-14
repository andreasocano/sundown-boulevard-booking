using System.Collections.Generic;

namespace SundownBoulevard.Booking.Website.Models
{
    public class MenuViewModel : ReservationViewModel
    {
        public MenuViewModel(string reservationID) : base(reservationID)
        {
        }

        public IEnumerable<Drink> Drinks { get; set; }
        public Dish Dish { get; set; }
    }
}