namespace SundownBoulevard.Booking.Website.Models
{
    public class ReservationViewModel
    {
        public ReservationViewModel(string reservationID)
        {
            ReservationID = reservationID;
        }
        public string ReservationID { get; set; }
    }
}