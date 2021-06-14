using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SundownBoulevard.Booking.Website.Controllers
{
    public class ReservationControllerBase : Controller
    {
        public string ReservationID;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ReservationID = HttpContext.Session[Globals.SessionKey.ReservationID]?.ToString();
            if (string.IsNullOrWhiteSpace(ReservationID)) filterContext.Result = Redirect("/");
            base.OnActionExecuting(filterContext);
        }
    }
}