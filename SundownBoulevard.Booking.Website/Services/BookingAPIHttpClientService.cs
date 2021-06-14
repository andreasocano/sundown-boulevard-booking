using Newtonsoft.Json;
using SundownBoulevard.Booking.Website.Factories;
using System;
using System.Net.Http;

namespace SundownBoulevard.Booking.Website.Service
{
    public static class BookingAPIHttpClientService
    {
        public static string Post(object request, string path)
        {
            using (var httpClient = new HttpClient(CertificateErrorHandlerFactory.Create()))
            {
                httpClient.BaseAddress = new Uri(Globals.AppSettings.APIDomain);
                var content = new StringContent(JsonConvert.SerializeObject(request));
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
                var response = httpClient.PostAsync(path, content).Result;
                var responseContent = response.Content.ReadAsStringAsync().Result;
                return responseContent;
            }
        }
    }
}