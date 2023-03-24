using RestSharpFinals.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFinals.Tests.TestData
{
    public class GenerateBooking
    {
        public static BookingModel BookingDetails()
        {
            return new BookingModel
            {
                Firstname = "Jim",
                Lastname = "Brown",
                Totalprice = 111,
                Depositpaid = true,
                Bookingdates = new Bookingdates()
                {
                    Checkin = new DateTime(2018-01-01),
                    Checkout = new DateTime(2018-01-01)
                },
                Additionalneeds = "Breakfast"
            };
        }
    }
}
