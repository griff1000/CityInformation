using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MyCorp.CityApi.Controllers;
using MyCorp.CityApi.Models.Database;
using MyCorp.CityApi.Options;
using MyCorp.CityApi.Persistence;
using MyCorp.CityApi.Services.Countries;
using MyCorp.CityApi.Services.Weather;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CityApi.UnitTests.Controllers
{
    using System.Collections;
    using Microsoft.EntityFrameworkCore;
    using MyCorp.CityApi.Data.DTO.Response;
    using MyCorp.CityApi.Models.Services.Countries;
    using MyCorp.CityApi.Models.Services.Weather;

    [TestClass]
    public class CitiesControllerTests
    {
        private Mock<IOptionsMonitor<AppSettingsOptions>> _appSettings;
        private Mock<ICityRepository> _cityRepository;
        private Mock<ICountryApiClient> _countryApiClient;
        private Mock<IMapper> _mapper;
        private Mock<IWeatherApiClient> _weatherApiClient;

        [TestInitialize]
        public void Startup()
        {
            _appSettings = new Mock<IOptionsMonitor<AppSettingsOptions>>();
            _cityRepository = new Mock<ICityRepository>();
            _countryApiClient = new Mock<ICountryApiClient>();
            _mapper = new Mock<IMapper>();
            _weatherApiClient = new Mock<IWeatherApiClient>();
        }

        [TestCleanup]
        public void Teardown()
        {

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsExceptionWithNullAppSettings()
        {
            _ = new CitiesController(null, _mapper.Object, _countryApiClient.Object, _weatherApiClient.Object, _cityRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsExceptionWithNullMapper()
        {
            _ = new CitiesController(_appSettings.Object, null, _countryApiClient.Object, _weatherApiClient.Object, _cityRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsExceptionWithNullCountryClient()
        {
            _ = new CitiesController(_appSettings.Object, _mapper.Object, null, _weatherApiClient.Object, _cityRepository.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsExceptionWithNullWeatherClient()
        {
            _ = new CitiesController(_appSettings.Object, _mapper.Object, _countryApiClient.Object, null, _cityRepository.Object);
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Ctor_ThrowsExceptionWithNullRepository()
        {
            _ = new CitiesController(_appSettings.Object, _mapper.Object, _countryApiClient.Object, _weatherApiClient.Object, null);
        }

        [TestMethod]
        public void GetAllCities_RedirectsToGetCitiesWithWildcard()
        {
            var c = new CitiesController(_appSettings.Object, _mapper.Object, _countryApiClient.Object, _weatherApiClient.Object, _cityRepository.Object);

            var result = c.GetAllCities();

            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            Assert.AreEqual("*", ((RedirectToActionResult)result).RouteValues["cityName"]);
        }



        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public async Task GetCities_ThrowsExceptionWithEmptyCity()
        {
            var c = new CitiesController(_appSettings.Object, _mapper.Object, _countryApiClient.Object, _weatherApiClient.Object, _cityRepository.Object);

            _ = await c.GetCities(string.Empty);

        }

        [TestMethod]
        public async Task GetCities_ReturnsEmptyListIfNoCities()
        {
            ICollection<City> cityList = new List<City>();
            _cityRepository.Setup(cr => cr.GetCitiesAsync(It.IsAny<string>())).ReturnsAsync(cityList);
            var c = new CitiesController(_appSettings.Object, _mapper.Object, _countryApiClient.Object, _weatherApiClient.Object, _cityRepository.Object);

            var response = await c.GetCities("mytown");

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
        }

        [TestMethod]
        public async Task GetCities_CallsServicesForEachCity()
        {
            ICollection<City> cityList = new List<City>();
            ICollection<Country> countryList = new List<Country>();
            cityList.Add(new City {CountryName = "United Kingdom", Name = "Cardiff"});
            _cityRepository.Setup(cr => cr.GetCitiesAsync(It.IsAny<string>())).ReturnsAsync(cityList);
            _mapper.Setup(m => m.Map<CityInformation>(It.IsAny<City>())).Returns(new CityInformation{CountryName = "United Kingdom", Name = "Cardiff"});
            _countryApiClient.Setup(countries => countries.GetCountriesAsync(It.IsAny<string>())).ReturnsAsync(countryList);
            _weatherApiClient.Setup(weather => weather.GetWeatherForCityAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(new Envelope {Weather = new Weather[] {new Weather{Description = "not too shabby"}}});
            _appSettings.SetupGet(appSettings => appSettings.CurrentValue)
                .Returns(new AppSettingsOptions {OpenweatherAppId = "12345"});
            
            var c = new CitiesController(_appSettings.Object, _mapper.Object, _countryApiClient.Object, _weatherApiClient.Object, _cityRepository.Object);

            var response = await c.GetCities("mytown");

            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkObjectResult));
            Assert.AreEqual("not too shabby", (((OkObjectResult)response).Value as List<CityInformation>).First().WeatherDescription);
            _weatherApiClient.Verify();
            _countryApiClient.Verify();
        }
    }
}
