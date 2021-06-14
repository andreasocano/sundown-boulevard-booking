using Newtonsoft.Json;
using SundownBoulevard.Booking.DTO.Models;
using SundownBoulevard.Booking.Website.Factories;
using SundownBoulevard.Booking.Website.Models;
using SundownBoulevard.Booking.Website.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Mvc;

namespace SundownBoulevard.Booking.Website.Controllers
{
    public class DateController : ReservationControllerBase
    {
        public ActionResult Index(int? year)
        {
            List<TimeSpan> allTimeSlots = GetAllTimeSlots();
            var currentDate = DateTime.Now;
            var currentYear = currentDate.Year;
            if (year != null && year > currentYear)
            {
                var referenceDate = new DateTime(year.Value, 1, 1);
                List<TimeSpan> specificTimeSlots = GetSpecificTimeSlots(year, referenceDate);
                return View(new DateViewModel(ReservationID)
                {
                    ReferenceDate = referenceDate,
                    IsForCurrentYear = false,
                    AllTimeSlots = allTimeSlots,
                    SpecificTimeSlots = specificTimeSlots
                });
            }
            List<TimeSpan> currentTimeSlots = GetSpecificTimeSlots(currentYear, currentDate);
            return View(new DateViewModel(ReservationID)
            {
                ReferenceDate = currentDate,
                AllTimeSlots = allTimeSlots,
                SpecificTimeSlots = currentTimeSlots
            });
        }

        [HttpPost]
        public ActionResult Index(SaveReservationDateRequest request) =>
            Json(BookingAPIHttpClientService.Post(request, "/reservation/date"));
    
        public ActionResult TimeSlots(int year, int month, int day)
        {
            var date = new DateTime(year, month, day);
            var specificTimeSlots = GetSpecificTimeSlots(year, date);
            return Json(specificTimeSlots.Select(sts => sts.Ticks));//hours = sts.Hours, minutes = sts.Minutes }));
        }

        public ActionResult Days(GetDateDaysRequest request)
        {
            var currentDate = DateTime.Now;
            var daysInMonth = DateTime.DaysInMonth(request.Year, request.Month);
            var response = new GetDateDaysResponse();
            for (int i = 1; i <= daysInMonth; i++)
            {
                var day = new DateTime(request.Year, request.Month, i);
                response.Items.Add(new GetDateDaysItem
                {
                    Text = $"{i} ({day.DayOfWeek})",
                    Value = i,
                    IsDisabled = request.Year == currentDate.Year
                        && request.Month == currentDate.Month
                        && i < currentDate.Day,
                    IsSelected = request.Year == currentDate.Year
                        && request.Month == currentDate.Day
                        && i == currentDate.Day
                });
            }
            return Json(response);
        }

        private List<TimeSpan> GetSpecificTimeSlots(int? year, DateTime referenceDate)
        {
            List<TimeSpan> specificTimeSlots = new List<TimeSpan>();
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri("https://localhost:44337");
                var content = new StringContent(JsonConvert.SerializeObject(new { uid = ReservationID, year = year.Value, month = referenceDate.Month, day = referenceDate.Day }));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = httpClient.PostAsync("/reservation/time-slots/available", content).Result;
                if (response.IsSuccessStatusCode)
                {
                    var item = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var timeSlots = JsonConvert.DeserializeObject<List<string>>(item);
                        foreach (var timeSlot in timeSlots)
                        {
                            if (TimeSpan.TryParse(timeSlot, out var ts)) specificTimeSlots.Add(ts);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: add logging
                    }
                }
            }

            return specificTimeSlots;
        }

        private static List<TimeSpan> GetAllTimeSlots()
        {
            List<TimeSpan> allTimeSlots = new List<TimeSpan>();
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri("https://localhost:44337");
                var response = httpClient.GetAsync("/reservation/time-slots").Result;
                if (response.IsSuccessStatusCode)
                {
                    var item = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var timeSlots = JsonConvert.DeserializeObject<List<string>>(item);
                        foreach (var timeSlot in timeSlots)
                        {
                            if (TimeSpan.TryParse(timeSlot, out var ts)) allTimeSlots.Add(ts);
                        }
                    }
                    catch (Exception ex)
                    {
                        //TODO: add logging
                    }
                }
            }

            return allTimeSlots;
        }
    }
}