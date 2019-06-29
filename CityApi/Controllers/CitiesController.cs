namespace MyCorp.CityApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.DTO.Request;
    using Data.DTO.Response;
    using Microsoft.AspNet.OData;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    using Models.Database;
    using Options;
    using Persistence;
    using Services.Countries;
    using Services.Weather;

    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IOptionsMonitor<AppSettingsOptions> _appSettings;
        private readonly ICityRepository _cityRepository;
        private readonly ICountryApiClient _countryApiClient;
        private readonly IMapper _mapper;
        private readonly IWeatherApiClient _weatherApiClient;

        public CitiesController(IOptionsMonitor<AppSettingsOptions> appSettings, IMapper mapper,
            ICountryApiClient countryApiClient, IWeatherApiClient weatherApiClient, ICityRepository cityRepository)
        {
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _countryApiClient = countryApiClient ?? throw new ArgumentNullException(nameof(countryApiClient));
            _weatherApiClient = weatherApiClient ?? throw new ArgumentNullException(nameof(weatherApiClient));
            _cityRepository = cityRepository ?? throw new ArgumentNullException(nameof(cityRepository));
        }

        // GET: api/Cities
        [HttpGet]
        [EnableQuery]
        public ActionResult<IEnumerable<CityInformation>> GetAllCities()
        {
            return RedirectToAction("GetCities", new {cityName = "*"});
        }

        // GET: api/Cities/Cardiff
        [HttpGet("{cityName}")]
        [EnableQuery]
        public async Task<ActionResult<IEnumerable<CityInformation>>> GetCities(string cityName)
        {
            if (string.IsNullOrEmpty(cityName)) throw new ArgumentNullException(nameof(cityName));

            var cities = await _cityRepository.GetCities(cityName).ToListAsync();
            var response = new List<CityInformation>();

            foreach (var city in cities)
            {
                var cityInfo = _mapper.Map<CityInformation>(city);
                var countries = await _countryApiClient.GetCountriesAsync(cityInfo.CountryName);
                var countryCode = string.Empty;
                if (countries.Count > 0)
                {
                    var country = countries.First();
                    cityInfo.ShortCountryCode = country.Alpha2Code;
                    cityInfo.LongCountryCode = country.Alpha3Code;
                    cityInfo.CountryName = country.Name;
                    cityInfo.CurrencyCodes = new List<string>(country.Currencies.Select(c => c.Code));
                    cityInfo.EstimatedPopulation = country.Population;

                    countryCode = $",{country.Alpha2Code}";
                }

                var weather = await _weatherApiClient.GetWeatherForCityAsync($"{cityInfo.Name}{countryCode}",
                    _appSettings.CurrentValue.OpenweatherAppId);
                cityInfo.WeatherDescription = weather.Weather.FirstOrDefault()?.Description;

                response.Add(cityInfo);
            }

            return response;
        }


        // PUT: api/Cities/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCity(Guid id, [FromBody] CityToUpdate cityToUpdate)
        {
            if (id == Guid.Empty) return BadRequest("Invalid City Id");
            if (cityToUpdate == null) return BadRequest("No update information supplied");

            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            var dbCity = await _cityRepository.GetCityAsync(id);
            if (dbCity == null) return NotFound();

            _mapper.Map(cityToUpdate, dbCity);

            _cityRepository.UpdateCity(dbCity);

            var result = await _cityRepository.SaveAsync();

            if (result) return NoContent();

            throw new ArgumentException("Unable to update a City");
        }

        // POST: api/Cities
        [HttpPost]
        public async Task<ActionResult<City>> CreateCity([FromBody] CityToAdd city)
        {
            if (city == null) return BadRequest("No City data supplied");

            if (!ModelState.IsValid) return new UnprocessableEntityObjectResult(ModelState);

            var dbCity = _mapper.Map<City>(city);
            await _cityRepository.CreateCityAsync(dbCity);

            var result = await _cityRepository.SaveAsync();

            var dtoCity = _mapper.Map<CityInformation>(dbCity);
            //TODO: This isn't a fully populated CityInformation object - it won't have country or weather information
            if (result) return CreatedAtAction("GetCities", new {cityName = city.Name}, dtoCity);

            throw new ApplicationException("Failed to create new city");
        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> DeleteCity(Guid id)
        {
            if (id == Guid.Empty) return BadRequest("No valid CityId supplied");
            var dbCity = await _cityRepository.GetCityAsync(id);
            if (dbCity == null) return NotFound();

            _cityRepository.DeleteCity(dbCity);
            var result = await _cityRepository.SaveAsync();

            if (result) return NoContent();

            throw new ArgumentException("Failed to delete city");
        }
    }
}