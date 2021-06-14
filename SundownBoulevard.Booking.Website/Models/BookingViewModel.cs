namespace SundownBoulevard.Booking.Website.Models
{
    public class BookingViewModel : ReservationViewModel
    {
        public BookingViewModel(string reservationID) : base(reservationID)
        {
        }

        public string Email { get; set; }
    }
}