using System.Net.Http;
using System.Web.Configuration;

namespace SundownBoulevard.Booking.Website
{
    public static class Globals
    {
        public static class SessionKey
        {
            public static string ReservationID = nameof(ReservationID);
        }

        public static class AppSettings
        {
            public static string APIDomain => WebConfigurationManager.AppSettings[nameof(APIDomain)];
            public static string DrinksURL => WebConfigurationManager.AppSettings[nameof(DrinksURL)];
            public static string DishURL => WebConfigurationManager.AppSettings[nameof(DishURL)];
        }
    }
}