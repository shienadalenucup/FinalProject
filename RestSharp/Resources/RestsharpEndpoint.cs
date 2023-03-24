using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharpFinals;
using RestSharpFinals.DataModels;

namespace RestSharpFinals.Resources
{
    public class RestsharpEndpoint
    {
        public const string BaseURL = "https://restful-booker.herokuapp.com/";
        public const string BookingEndpoint = "booking";
        public const string AuthEndpoint = "auth";
        public static string BaseBooking => $"{BaseURL}{BookingEndpoint}";
        public static string GenerateToken => $"{BaseURL}{AuthEndpoint}";
        public static string BookingById(int bookingId) => $"{BaseBooking}/{bookingId}";
    }
}
