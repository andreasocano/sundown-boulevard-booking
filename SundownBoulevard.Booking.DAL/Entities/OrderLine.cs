namespace SundownBoulevard.Booking.DAL.Entities
{
    public class OrderLine
    {
        public int ID { get; set; }
        public string Item { get; set; }
        public int OrderID { get; set; }
        public virtual Order Order { get; set; }
    }
}
