namespace MyCorp.CityApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Data.DTO.Request;
    using Data.DTO.Response;
    using Database;
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
        private readonly ApiContext _context;
        private readonly IMapper _mapper;
        private readonly ICountryApiClient _countryApiClient;
        private readonly IWeatherApiClient _weatherApiClient;
        private readonly ICityRepository _cityRepository;

        public CitiesController(ApiContext context, IOptionsMonitor<AppSettingsOptions> appSettings, IMapper mapper,
            ICountryApiClient countryApiClient, IWeatherApiClient weatherApiClient, ICityRepository cityRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
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
        public async Task<IActionResult> PutCity(Guid cityId, City city)
        {
            if (cityId != city.CityId) return BadRequest();

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(cityId))
                    return NotFound();
                throw;
            }

            return NoContent();
        }

        // POST: api/Cities
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(CityToAdd city)
        {
            var dbCity = Mapper.Map<City>(city);
            await _cityRepository.CreateCityAsync(dbCity);
            var result = await _cityRepository.SaveAsync();

            if (result)
            {
                return CreatedAtAction("GetCities", new {cityName = city.Name}, city);
            }
            throw new ApplicationException("Failed to create new city");

        }

        // DELETE: api/Cities/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<City>> DeleteCity(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null) return NotFound();

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return city;
        }

        private bool CityExists(Guid id)
        {
            return _context.Cities.Any(e => e.CityId == id);
        }
    }
}