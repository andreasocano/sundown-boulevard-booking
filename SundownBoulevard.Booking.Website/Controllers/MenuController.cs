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
    public class MenuController : ReservationControllerBase
    {
        public ActionResult Index()
        {
            IEnumerable<Drink> drinks = GetDrinks();
            var dish = GetDish();
            return View(new MenuViewModel(ReservationID)
            {
                Drinks = drinks,
                Dish = dish
            });
        }

        [HttpPost]
        public ActionResult Index(SaveMenuRequest request) =>
            Json(BookingAPIHttpClientService.Post(request, "/reservation/menu"));

        [HttpPost]
        public ActionResult Dish() => Json(GetDish());

        private IEnumerable<Drink> GetDrinks()
        {
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri(Globals.AppSettings.DrinksURL);
                var response = httpClient.GetAsync(string.Empty).Result;
                if (response.IsSuccessStatusCode)
                {
                    var item = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var drinksObject = JsonConvert.DeserializeObject<List<object>>(item);
                        var drinks = new List<Drink>();
                        foreach (var drinkObject in drinksObject)
                        {
                            var drinkContent = drinkObject.ToString();
                            var drink = JsonConvert.DeserializeObject<Drink>(drinkContent);
                            drink.Object = drinkContent;
                            drinks.Add(drink);
                        }
                        return drinks;
                    }
                    catch (Exception ex)
                    {
                        //TODO: log
                    }
                }
            }

            return Enumerable.Empty<Drink>();
        }

        private Dish GetDish()
        {
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri(Globals.AppSettings.DishURL);
                var response = httpClient.GetAsync(string.Empty).Result;
                if (response.IsSuccessStatusCode)
                {
                    var item = response.Content.ReadAsStringAsync().Result;
                    try
                    {
                        var dishWrapper = JsonConvert.DeserializeObject<DishWrapper>(item);
                        var dish = dishWrapper.Dishes.FirstOrDefault() ?? new Dish();
                        dish.Object = item;
                        return dish;
                    }
                    catch (Exception ex)
                    {
                        //TODO: add logging
                    }
                }
            }

            return new Dish();
        }
    }
}