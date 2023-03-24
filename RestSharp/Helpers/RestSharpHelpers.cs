using RestSharpFinals.DataModels;
using RestSharpFinals.Resources;
using RestSharpFinals.Tests.TestData;
using RestSharp;

namespace RestSharpFinals.Helpers
{
    /// <summary>
    /// Contains all methods for booking
    /// </summary>
    public class RestSharpHelpers
    {
        private static async Task<string> GetToken(RestClient restClient)
        {
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");

            var postRequest = new RestRequest(RestsharpEndpoint.GenerateToken).AddJsonBody(AuthTokenInfo.Details());

            var generateToken = await restClient.ExecutePostAsync<TokenModel>(postRequest);

            return generateToken.Data.Token;
        }

        /// <summary>
        /// Send Post request
        /// </summary>
        /// <param name="restClient"></param>
        /// <returns></returns>
        public static async Task<RestResponse<BookingModelId>> CreateBooking(RestClient restClient)
        {
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");

            //var postRequest = new RestRequest(RestsharpEndpoint.BaseBooking).AddJsonBody(GenerateBookingDetails.bookingDetails());
            var postRequest = new RestRequest(RestsharpEndpoint.BaseBooking).AddJsonBody(GenerateBooking.BookingDetails());

            return await restClient.ExecutePostAsync<BookingModelId>(postRequest);
        }

        /// <summary>
        /// Send GET request
        /// </summary>
        /// <param name="restClient"></param>
        /// <param name="bookingId"></param>
        /// <returns></returns>
        public static async Task<RestResponse<BookingModel>> GetBooking(RestClient restClient, int bookingId)
        {
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");

            var getRequest = new RestRequest(RestsharpEndpoint.BookingById(bookingId));

            return await restClient.ExecuteGetAsync<BookingModel>(getRequest);
        }

        /// <summary>
        /// Method in deleting the booking data
        /// </summary>
        public static async Task<RestResponse> DeleteBooking(RestClient restClient, int bookingId)
        {
            var token = await GetToken(restClient);
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");
            restClient.AddDefaultHeader("Cookie", "token=" + token);

            var getRequest = new RestRequest(RestsharpEndpoint.BookingById(bookingId));

            return await restClient.DeleteAsync(getRequest);
        }

        public static async Task<RestResponse<BookingModel>> UpdateBooking(RestClient restClient, BookingModel booking, int bookingId)
        {
            var token = await GetToken(restClient);
            restClient = new RestClient();
            restClient.AddDefaultHeader("Accept", "application/json");
            restClient.AddDefaultHeader("Cookie", "token=" + token);

            var putRequest = new RestRequest(RestsharpEndpoint.BookingById(bookingId)).AddJsonBody(booking);

            return await restClient.ExecutePutAsync<BookingModel>(putRequest);
        }
    }    
}

