using Microsoft.VisualStudio.TestTools.UnitTesting;
using RestSharp;
using RestSharpFinals.DataModels;

namespace RestSharpFinals
{
    public class ApiBaseTest
    {
        public RestClient RestClient { get; set; }

        public BookingModelId BookingDetails { get; set; }

        [TestInitialize]
        public void Initilize()
        {
            RestClient = new RestClient();
        }
    }
}