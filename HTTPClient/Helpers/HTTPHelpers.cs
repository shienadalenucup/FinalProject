using HTTPClient.DataModels;
using HTTPClient.Resources;
using HTTPClient.Tests.TestData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HTTPClient.Helpers
{
    /// <summary>
    /// Contains all methods for booking
    /// </summary>
    public class HTTPHelpers
    {
        private HttpClient httpClient;

        public void AcceptJsonOnHeader()
        {
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        }

        public async Task AuthenticateRequest()
        {
            var token = await GetToken();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"token={token}");
        }

        /// <summary>
        /// 
        /// </summary>
        private async Task<string> GetToken()
        {
            AcceptJsonOnHeader();

            // Serialize Content
            var request = JsonConvert.SerializeObject(AuthTokenInfo.Details());
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            var httpResponse = await httpClient.PostAsync(HTTPClientEndpoint.GetURL(HTTPClientEndpoint.AuthEndpoint), postRequest); 

            // Deserialize Content
            var token = JsonConvert.DeserializeObject<TokenModel>(httpResponse.Content.ReadAsStringAsync().Result);

            // Return Token
            return token.Token;
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<HttpResponseMessage> CreateBooking()
        {
            AcceptJsonOnHeader();

            // Serialize Content
            var request = JsonConvert.SerializeObject(GenerateBooking.BookingDetails());
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Return Send Request
            return await httpClient.PostAsync(HTTPClientEndpoint.GetURL(HTTPClientEndpoint.BookingEndpoint), postRequest);
        }

        /// <summary>
        ///
        /// </summary>
        public async Task<HttpResponseMessage> GetBooking(int bookingId)
        {
            AcceptJsonOnHeader();

            // Return Get Request
            return await httpClient.GetAsync(HTTPClientEndpoint.GetURI($"{HTTPClientEndpoint.BookingEndpoint}/{bookingId}"));
        }

        /// <summary>
        ///
        /// </summary>
        public async Task<HttpResponseMessage> UpdateBooking(BookingModel bookingDetails, int bookingId)
        {
            var token = await GetToken();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"token={token}");

            // Serialize Content
            var request = JsonConvert.SerializeObject(bookingDetails);
            var putRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Return Put Request
            return await httpClient.PutAsync(HTTPClientEndpoint.GetURL($"{HTTPClientEndpoint.BookingEndpoint}/{bookingId}"), putRequest);
        }

        /// <summary>
        /// 
        /// </summary>
        public async Task<HttpResponseMessage> DeleteBooking(int bookingId)
        {
            var token = await GetToken();
            httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Add("Cookie", $"token={token}");

            // Return Delete Request
            return await httpClient.DeleteAsync(HTTPClientEndpoint.GetURL($"{HTTPClientEndpoint.BookingEndpoint}/{bookingId}"));
        }
    }
}
