using System;
using System.Collections.Generic;
using System.Text;

namespace CityApi.UnitTests.Services.Countries
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using MyCorp.CityApi.Options;
    using MyCorp.CityApi.Services.Countries;
    using TestHelpers;

    [TestClass]
    public class CountryApiClientTests
    {
        private Mock<IOptionsMonitor<AppSettingsOptions>> _appSettings;

        [TestInitialize]
        public void Startup()
        {
            _appSettings = new Mock<IOptionsMonitor<AppSettingsOptions>>();
        }

        [TestCleanup]
        public void Teardown()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsExceptionWithNullClient()
        {
            _ = new CountryApiClient(null, _appSettings.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsExceptionWithNullAppSettings()
        {
            _ = new CountryApiClient(new HttpClient(), null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetCountriesAsync_ThrowsExceptionWithEmptyCountry()
        {
            var client = new CountryApiClient(new HttpClient(), _appSettings.Object);
            await client.GetCountriesAsync(String.Empty);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ApplicationException))]
        public async Task GetCountriesAsync_ThrowsExceptionWithUnsuccessfulGet()
        {
            _appSettings.SetupGet(a => a.CurrentValue)
                .Returns(new AppSettingsOptions {CountryApiUrl = "http://mytest.com/?id={0}"});
            var testHandler =
                new TestHttpClientMessageHandler(HttpStatusCode.InternalServerError, "something happened");
            using (var httpClient = new HttpClient(testHandler))
            {
                var client = new CountryApiClient(httpClient, _appSettings.Object);
                await client.GetCountriesAsync("Wales");
            }
        }



        [TestMethod]
        public async Task GetCountriesAsync_ReturnsDeserializedData()
        {
            _appSettings.SetupGet(a => a.CurrentValue)
                .Returns(new AppSettingsOptions { CountryApiUrl = "http://mytest.com/?id={0}" });
            var testHandler =
                new TestHttpClientMessageHandler(HttpStatusCode.OK, TestResponses.ValidCountryResponse);
            using (var httpClient = new HttpClient(testHandler))
            {
                var client = new CountryApiClient(httpClient, _appSettings.Object);
                var result = await client.GetCountriesAsync("United Kingdom");

                Assert.AreEqual(1, result.Count);
                Assert.AreEqual("GBR", result.First().Alpha3Code);

                //TODO: Perhaps not ideal to test output and behaviour in the same
                //test but it saves a lot of code!
                _appSettings.VerifyGet(a => a.CurrentValue, Times.Once);
            }
        }
    }
}
