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
    public class LoginController : ReservationControllerBase
    {
        public ActionResult Index()
        {
            return View(new ReservationViewModel(ReservationID));
        }

        public ActionResult Confirm(ActivateReservationRequest request) =>
            Json(BookingAPIHttpClientService.Post(request, "/reservation/activate"));
    }
}