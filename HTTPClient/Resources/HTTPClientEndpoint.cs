using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Resources
{
    public class HTTPClientEndpoint
    {
        public const string BaseURL = "https://restful-booker.herokuapp.com/";
        public const string BookingEndpoint = "booking";
        public const string AuthEndpoint = "auth";


        public static string GetURL(string endpoint) => $"{BaseURL}{endpoint}";
        //public static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));
        public static Uri GetURI(string endpoint) => new (GetURL(endpoint));
    }
}
