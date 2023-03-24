using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFinals.DataModels
{
    public class BookingModelId
    {
        [JsonProperty("bookingid")]
        public int Bookingid { get; set; }

        [JsonProperty("booking")]
        public BookingModel Booking { get; set; }
    }
}
