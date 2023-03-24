using Microsoft.VisualStudio.TestTools.UnitTesting;
using ServiceReference1;
using System;
using System.Security.Cryptography.X509Certificates;

[assembly: Parallelize(Workers = 10, Scope = ExecutionScope.MethodLevel)]

namespace SOAP
{
    [TestClass]
    public class CountryInfoServiceTest
    {
        //Global Variable
        private readonly CountryInfoServiceSoapTypeClient countryInfoServiceTest =
            new CountryInfoServiceSoapTypeClient(CountryInfoServiceSoapTypeClient.EndpointConfiguration.CountryInfoServiceSoap);

        //Arrange
        //Pre-requisites
        private tCountryCodeAndName[] CountryNameAndCodeList()
        {
            var countryList = countryInfoServiceTest.ListOfCountryNamesByCode();

            return countryList;
        }

        private static tCountryCodeAndName RandomCountryCode(tCountryCodeAndName[] countryList)
        {
            Random rdm = new Random();
            int countryCount = countryList.Count() - 1;
            int randomRecord = rdm.Next(0, countryCount);
            var randomCountryCode = countryList[randomRecord];

            return randomCountryCode;
        }

        //Act
        [TestMethod]
        public void FullCountryInfo()
        {
            var countryList = CountryNameAndCodeList();
            var countryListCode = RandomCountryCode(countryList);
            var countryDetails = countryInfoServiceTest.FullCountryInfo(countryListCode.sISOCode);

        //Assert
            Assert.AreEqual(countryListCode.sISOCode, countryDetails.sISOCode, "Country code doesn't match.");
            Assert.AreEqual(countryListCode.sName, countryDetails.sName, "Country name doesn't match.");
        }

        [TestMethod]
        public void SelectRandomCountryISOCode()
        {
            var countryList = CountryNameAndCodeList();
            List<tCountryCodeAndName> fiveRandomCountry = new List<tCountryCodeAndName>();

            for (int x = 0; x < 5; x++)
            {
                fiveRandomCountry.Add(RandomCountryCode(countryList));
            }

            foreach (var country in fiveRandomCountry)
            {
                var countryISOCode = countryInfoServiceTest.CountryISOCode(country.sName);

                //Assert
                Assert.AreEqual(country.sISOCode, countryISOCode, "Country code doesn't match.");
            }
        }
    }
}