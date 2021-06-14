using Newtonsoft.Json;
using SundownBoulevard.Booking.Website.Factories;
using SundownBoulevard.Booking.Website.Models;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace SundownBoulevard.Booking.Website.Controllers
{
    public class BookingController : ReservationControllerBase
    {
        public ActionResult Index()
        {
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri($"https://localhost:44337/reservation/email/{ReservationID}");
                var result = httpClient.GetAsync(string.Empty).Result;
                if (!result.IsSuccessStatusCode)
                    return View();
                var email = result.Content.ReadAsStringAsync().Result;
                return View(new BookingViewModel(ReservationID) { Email = email });
            }
        }

        public ActionResult Confirm(string activationCode)
        {
            if (string.IsNullOrWhiteSpace(activationCode))
                return Index();
            var uid = ReservationID;
            HttpContext.Session[Globals.SessionKey.ReservationID] = null;
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri("https://localhost:44337");
                var content = new StringContent(JsonConvert.SerializeObject(new { activationCode, uid }));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var result = httpClient.PostAsync("/booking/confirm", content).Result;
                if (!result.IsSuccessStatusCode)
                    return View("Expired");
            }

            return View("Receipt");
        }
    }
}