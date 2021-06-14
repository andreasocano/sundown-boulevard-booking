using Newtonsoft.Json;
using SundownBoulevard.Booking.DTO.Models;
using SundownBoulevard.Booking.Website.Factories;
using SundownBoulevard.Booking.Website.Models;
using SundownBoulevard.Booking.Website.Service;
using System;
using System.Net.Http;
using System.Web.Mvc;

namespace SundownBoulevard.Booking.Website.Controllers
{
    public class GuestsController : Controller
    {
        public ActionResult Index()
        {
            if (HttpContext.Session[Globals.SessionKey.ReservationID] is string reservationID)
                return View(new ReservationViewModel(reservationID));
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri(Globals.AppSettings.APIDomain);
                var result = httpClient.GetAsync("/reservation/initialize").Result;
                reservationID = result.Content.ReadAsStringAsync().Result.Trim('"');
                if (string.IsNullOrWhiteSpace(reservationID))
                {
                    throw new NullReferenceException("Reservations require an ID");
                }
                HttpContext.Session[Globals.SessionKey.ReservationID] = reservationID;
                return View(new ReservationViewModel(reservationID));
            }
        }

        public ActionResult Seats(SaveReservationSeatsRequest request) =>
            Json(BookingAPIHttpClientService.Post(request, "/reservation/seats"));
    }
}