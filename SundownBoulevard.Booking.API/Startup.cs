using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SundownBoulevard.Booking.API.Factories;
using SundownBoulevard.Booking.API.Models;
using SundownBoulevard.Booking.API.Repositories;
using SundownBoulevard.Booking.API.Services;
using SundownBoulevard.Booking.DAL.Entities;
using SundownBoulevard.Booking.DAL.Repositories;
using System;
using System.Linq;

namespace SundownBoulevard.Booking.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowWebsite", builder => builder.WithOrigins(Configuration["WebsiteDomain"])
                    .AllowAnyMethod()
                    .AllowAnyHeader());
            });

            string connectionString = Configuration.GetValue<string>("DatabaseContext:ConnectionString");
            services.AddDbContext<RestaurantContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddControllers();
            services.AddTransient<RestaurantContext>();
            services.AddTransient<ReservationRepository>();
            services.AddTransient<BookingRepository>();
            services.AddTransient<ActivateReservationService>();
            services.AddTransient<IsValidReservationService>();
            var smtpConfiguration = Configuration
             .GetSection("SMTPConfiguration")
             .Get<SMTPConfiguration>();
            services.AddSingleton(smtpConfiguration);
            services.AddSingleton<SendEmailService>();
            services.AddTransient<FinalizeBookingService>();
            services.AddTransient<TimeSlotFactory>();
            services.AddTransient<TableScheduleRepository>();
            services.AddTransient<TableRepository>();
            services.AddTransient<HasSufficientAmountOfSeatsService>();
            services.AddTransient<TableReservationRepository>();
            services.AddTransient<OrderRepository>();
            var bookingConfiguration = Configuration
             .GetSection("BookingConfiguration")
             .Get<BookingConfiguration>();
            services.AddSingleton(bookingConfiguration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger, RestaurantContext databaseContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            MigrateDatabase(databaseContext, logger);

            SeedDatabase(databaseContext);
        }

        private static void SeedDatabase(RestaurantContext databaseContext)
        {
            if (!databaseContext.Tables.Any())
            {
                for (int i = 0; i < 10; i++)
                {
                    var name = Convert.ToChar(65 + i).ToString();
                    databaseContext.Tables.Add(new Table { Name = name, Seats = 2, CreatedDate = DateTime.Now });
                }
                databaseContext.SaveChanges();
            }
        }

        private void MigrateDatabase(RestaurantContext databaseContext, ILogger<Startup> logger)
        {
            var canConnect = databaseContext.Database.CanConnect();
            if (!canConnect) throw new Exception("Cannot connect to database");

            try
            {
                logger.LogInformation("Init Database Migrate");
                databaseContext.Database.Migrate();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Unable to upgrade database.");
            }
        }
    }
}
