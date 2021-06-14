using Microsoft.EntityFrameworkCore;

namespace SundownBoulevard.Booking.DAL.Entities
{
    public class RestaurantContext : DbContext
	{
		public RestaurantContext(DbContextOptions<RestaurantContext> dbContextOptions) : base(dbContextOptions)
		{
		}
		public DbSet<Table> Tables { get; set; }
		public DbSet<Reservation> Reservations { get; set; }
		public DbSet<Booking> Bookings { get; set; }
		public DbSet<TableReservation> TableReservations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderLine> OrderLines { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
		}
	}
}
