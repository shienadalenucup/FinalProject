using RestSharpFinals.DataModels;
using RestSharpFinals.Helpers;
using RestSharpFinals.Resources;
using RestSharpFinals.Tests.TestData;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using RestSharp.Serializers;
using RestSharpFinals.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using RestSharp;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]

namespace RestSharpFinals.Tests
{
    [TestClass]
    public class RestSharpTests : ApiBaseTest
    {
        private readonly List<BookingModelId> cleanupList = new List<BookingModelId>();

        [TestInitialize]
        public async Task TestInitialize()
        {
            // Create Data
            var restResponse = await RestSharpHelpers.CreateBooking(RestClient);
            BookingDetails = restResponse.Data;

            // Assertion
            Assert.AreEqual(restResponse.StatusCode, HttpStatusCode.OK);
        }

        [TestCleanup]
        public async Task TestCleanup()
        {
            foreach (var data in cleanupList)
            {
                var deleteBookingResponse = await RestSharpHelpers.DeleteBooking(RestClient, data.Bookingid);
            }
        }

        /// <summary>
        /// Test method that creates a booking
        /// </summary>
        [TestMethod]
        public async Task CreateBooking()
        {
            // Create Data
            var listBookingData = await RestSharpHelpers.GetBooking(RestClient, BookingDetails.Bookingid);

            // Clean Up
            cleanupList.Add(BookingDetails);

            // Assertion
            var createdBookingData = GenerateBooking.BookingDetails();

            Assert.AreEqual(createdBookingData.Firstname, listBookingData.Data.Firstname, "Firstname does not match.");
            Assert.AreEqual(createdBookingData.Lastname, listBookingData.Data.Lastname, "Lastname does not match.");
            Assert.AreEqual(createdBookingData.Totalprice, listBookingData.Data.Totalprice, "Total price does not match.");
            Assert.AreEqual(createdBookingData.Depositpaid, listBookingData.Data.Depositpaid, "Deposit paid does not match.");
            Assert.AreEqual(createdBookingData.Bookingdates.Checkin.Date, listBookingData.Data.Bookingdates.Checkin.Date, "Check-in date does not match.");
            Assert.AreEqual(createdBookingData.Bookingdates.Checkout.Date, listBookingData.Data.Bookingdates.Checkout.Date, "Check-out date does not match.");
            Assert.AreEqual(createdBookingData.Additionalneeds, listBookingData.Data.Additionalneeds, "Additional needs does not match.");
        }

        /// <summary>
        /// Test Method in updating the first and last name of a booking
        /// </summary>
        [TestMethod]
        public async Task UpdateBooking()
        {
            // Create Data
            var listBookingData = await RestSharpHelpers.GetBooking(RestClient, BookingDetails.Bookingid);

            // Clean Up
            cleanupList.Add(BookingDetails);

            // Update Data 
            var updatedBookingInfo = new BookingModel()
            {
                Firstname = "Modified Firstname",
                Lastname = "Modified Lastname",
                Totalprice = listBookingData.Data.Totalprice,
                Depositpaid = listBookingData.Data.Depositpaid,
                Bookingdates = listBookingData.Data.Bookingdates,
                Additionalneeds = listBookingData.Data.Additionalneeds
            };
            var updateBooking = await RestSharpHelpers.UpdateBooking(RestClient, updatedBookingInfo, BookingDetails.Bookingid);

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, updateBooking.StatusCode);

            // Get Updated Data 
            var getUpdatedBookingResponse = await RestSharpHelpers.GetBooking(RestClient, BookingDetails.Bookingid);

            // Assertion
            Assert.AreEqual(updatedBookingInfo.Firstname, getUpdatedBookingResponse.Data.Firstname, "Firstname does not match.");
            Assert.AreEqual(updatedBookingInfo.Lastname, getUpdatedBookingResponse.Data.Lastname, "Lastname does not match.");
            Assert.AreEqual(updatedBookingInfo.Totalprice, getUpdatedBookingResponse.Data.Totalprice, "Total price does not match.");
            Assert.AreEqual(updatedBookingInfo.Depositpaid, getUpdatedBookingResponse.Data.Depositpaid, "Deposit paid does not match.");
            Assert.AreEqual(updatedBookingInfo.Bookingdates.Checkin.Date, getUpdatedBookingResponse.Data.Bookingdates.Checkin.Date, "Check-in date does not match.");
            Assert.AreEqual(updatedBookingInfo.Bookingdates.Checkout.Date, getUpdatedBookingResponse.Data.Bookingdates.Checkout.Date, "Check-out date does not match.");
            Assert.AreEqual(updatedBookingInfo.Additionalneeds, getUpdatedBookingResponse.Data.Additionalneeds, "Additional needs does not match.");
        }

        /// <summary>
        /// Test Method in deleting a booking
        /// </summary>
        [TestMethod]
        public async Task DeleteBooking()
        {
            // Delete Data
            var deleteBooking = await RestSharpHelpers.DeleteBooking(RestClient, BookingDetails.Bookingid);

            // Assertion
            Assert.AreEqual(HttpStatusCode.Created, deleteBooking.StatusCode);
        }

        /// <summary>
        /// Test method to check whether booking is invalid
        /// </summary>
        [TestMethod]
        public async Task InvalidBooking()
        {
            // Create Data
            var getCreatedBooking = await RestSharpHelpers.GetBooking(RestClient, 20000000);

            // Clean up
            cleanupList.Add(BookingDetails);

            // Assertion
            Assert.AreEqual(HttpStatusCode.NotFound, getCreatedBooking.StatusCode);


        }
    }
}
