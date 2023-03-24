using HTTPClient.DataModels;
using HTTPClient.Helpers;
using HTTPClient.Resources;
using HTTPClient.Tests.TestData;
using Newtonsoft.Json;
using RestSharp.Serializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]

namespace HTTPClient.Tests
{
    [TestClass]
    public class HTTPClientTests
    {
        public HTTPHelpers hTTPHelpers;
        public HttpClient httpClient;

        //private readonly List<BookingModelId> cleanUpList = new List<BookingModelId>();
        private readonly List<BookingModelId> cleanUpList = new();

        [TestInitialize]
        public async Task TestInitialize()
        {
            hTTPHelpers = new HTTPHelpers();
            httpClient = new HttpClient();
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(HTTPClientEndpoint.GetURL($"{HTTPClientEndpoint.BookingEndpoint}/{data.Bookingid}"));
            }
        }

        /// <summary>
        /// Test Method in creating a booking
        /// </summary>
        [TestMethod]
        public async Task CreateBooking()
        {
            // Create Data
            var addBooking = await hTTPHelpers.CreateBooking();
            var getResponse = JsonConvert.DeserializeObject<BookingModelId>(addBooking.Content.ReadAsStringAsync().Result);

            // Add data to cleanup list
            cleanUpList.Add(getResponse);

            // Get Request 
            var getBooking = await hTTPHelpers.GetBooking(getResponse.Bookingid);

            //Deserialize Content
            var listBookingData = JsonConvert.DeserializeObject<BookingModel>(getBooking.Content.ReadAsStringAsync().Result);

            var createdBookingData = GenerateBooking.BookingDetails();

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, addBooking.StatusCode, "Failed due to wrong status code.");
            Assert.AreEqual(createdBookingData.Firstname, listBookingData.Firstname, "Firstname does not match.");
            Assert.AreEqual(createdBookingData.Lastname, listBookingData.Lastname, "Lastname does not match.");
            Assert.AreEqual(createdBookingData.Totalprice, listBookingData.Totalprice, "Total price does not match.");
            Assert.AreEqual(createdBookingData.Depositpaid, listBookingData.Depositpaid, "Deposit paid does not match.");
            Assert.AreEqual(createdBookingData.Bookingdates.Checkin.Date, listBookingData.Bookingdates.Checkin.Date, "Check-in date does not match.");
            Assert.AreEqual(createdBookingData.Bookingdates.Checkout.Date, listBookingData.Bookingdates.Checkout.Date, "Check-out date does not match.");
            Assert.AreEqual(createdBookingData.Additionalneeds, listBookingData.Additionalneeds, "Additional needs does not match.");
        }

        /// <summary>
        /// Test Method in updating the first and last name of a booking
        /// </summary>
        [TestMethod]
        public async Task UpdateBooking()
        {

            // Create Data
            var addBooking = await hTTPHelpers.CreateBooking();
            var getResponse = JsonConvert.DeserializeObject<BookingModelId>(addBooking.Content.ReadAsStringAsync().Result);

            // Add data to cleanup list
            cleanUpList.Add(item: getResponse);

            // Get Request 
            var getBooking = await hTTPHelpers.GetBooking(getResponse.Bookingid);

            //Deserialize Content
            var listBookingData = JsonConvert.DeserializeObject<BookingModel>(getBooking.Content.ReadAsStringAsync().Result);

            // Update Data
            var updatedBookingInfo = new BookingModel()
            {
                Firstname = "Modified Firstname",
                Lastname = "Modified Lastname",
                Totalprice = listBookingData.Totalprice,
                Depositpaid = listBookingData.Depositpaid,
                Bookingdates = listBookingData.Bookingdates,
                Additionalneeds = listBookingData.Additionalneeds
            };

            var updateBooking = await hTTPHelpers.UpdateBooking(updatedBookingInfo, getResponse.Bookingid);
            var getUpdatedResponse = JsonConvert.DeserializeObject<BookingModel>(updateBooking.Content.ReadAsStringAsync().Result);

            // Get Updated Request
            var getUpdatedBooking = await hTTPHelpers.GetBooking(getResponse.Bookingid);

            //Deserialize Content
            var getUpdatedBookingResponse = JsonConvert.DeserializeObject<BookingModel>(getUpdatedBooking.Content.ReadAsStringAsync().Result);

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, updateBooking.StatusCode);

            Assert.AreEqual(updatedBookingInfo.Firstname, getUpdatedBookingResponse.Firstname, "Firstname does not match.");
            Assert.AreEqual(updatedBookingInfo.Lastname, getUpdatedBookingResponse.Lastname, "Lastname does not match.");
            Assert.AreEqual(updatedBookingInfo.Totalprice, getUpdatedBookingResponse.Totalprice, "Total price does not match.");
            Assert.AreEqual(updatedBookingInfo.Depositpaid, getUpdatedBookingResponse.Depositpaid, "Deposit paid does not match.");
            Assert.AreEqual(updatedBookingInfo.Bookingdates.Checkin.Date, getUpdatedBookingResponse.Bookingdates.Checkin.Date, "Check-in date does not match.");
            Assert.AreEqual(updatedBookingInfo.Bookingdates.Checkout.Date, getUpdatedBookingResponse.Bookingdates.Checkout.Date, "Check-out date does not match.");
            Assert.AreEqual(updatedBookingInfo.Additionalneeds, getUpdatedBookingResponse.Additionalneeds, "Additional needs does not match.");
        }

        /// <summary>
        /// Test Method in deleting a booking
        /// </summary>
        [TestMethod]
        public async Task DeleteBooking()
        {
            // Create Data
            var addBooking = await hTTPHelpers.CreateBooking();
            var getResponse = JsonConvert.DeserializeObject<BookingModelId>(addBooking.Content.ReadAsStringAsync().Result);

            // Add data to cleanup list
            cleanUpList.Add(getResponse);

            // Get Request
            var getBooking = await hTTPHelpers.GetBooking(getResponse.Bookingid);

            //Deserialize Content
            var getBookingResponse = JsonConvert.DeserializeObject<BookingModel>(getBooking.Content.ReadAsStringAsync().Result);

            // Delete Data
            var deleteBooking = await hTTPHelpers.DeleteBooking(getResponse.Bookingid);

            // Assertion
            Assert.AreEqual(HttpStatusCode.Created, deleteBooking.StatusCode);
        }

        /// <summary>
        /// Test Method that checks whether the booking is invalid
        /// </summary>
        [TestMethod]
        public async Task InvalidBooking()
        {
            // Get Data 
            var getBooking = await hTTPHelpers.GetBooking(2000000000);

            // Assertion
            Assert.AreEqual(HttpStatusCode.NotFound, getBooking.StatusCode);
        }
    }
}
