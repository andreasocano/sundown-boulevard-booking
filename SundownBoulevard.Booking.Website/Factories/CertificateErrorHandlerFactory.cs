using System.Net.Http;

namespace SundownBoulevard.Booking.Website.Factories
{
    public static class CertificateErrorHandlerFactory
    {
        public static HttpClientHandler Create()
        {
            return new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
                {
                    return true;
                }
            };
        }
    }
}