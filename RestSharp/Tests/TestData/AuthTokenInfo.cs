using RestSharpFinals.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestSharpFinals.Tests.TestData
{
    public class AuthTokenInfo
    {
        public static TokenInfoModel Details()
        {
            return new TokenInfoModel
            {
                Username = "admin",
                Password = "password123"
            };
        }
    }
}
