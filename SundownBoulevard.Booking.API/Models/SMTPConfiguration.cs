namespace SundownBoulevard.Booking.API.Models
{
    public class SMTPConfiguration
    {
        public string Host { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string Sender { get; set; }
        public int Port { get; set; }
    }
}
