namespace MyCorp.CityApi.Services.Weather
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Models.Services.Weather;
    using Newtonsoft.Json;
    using Options;

    /// <summary>
    /// Handles interaction with the OpenWeather API
    /// </summary>
    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _client;
        private readonly IOptionsMonitor<AppSettingsOptions> _appSettings;

        public WeatherApiClient(HttpClient client, IOptionsMonitor<AppSettingsOptions> appSettings)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        /// <summary>
        /// Gets the weather for the given city
        /// </summary>
        /// <param name="city">Either just the name of the city, or city name concatenated with country code e.g. "Cardiff" or "Cardiff,GB"</param>
        /// <param name="appId">A valid OpenWeather API key</param>
        /// <returns>Weather information for the first city that matches the given city name</returns>
        public async Task<Envelope> GetWeatherForCityAsync(string city, string appId)
        {
            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
            if (string.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));

            city = city.Replace(" ", "%20");

            var apiUrl = string.Format(_appSettings.CurrentValue.OpenweatherApiUrl, city, appId);

            var response = await _client.GetAsync(apiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<Envelope>(responseContent);

            throw new ApplicationException($"Failed to call Weather API.  Status code: {response.StatusCode}");
        }
    }
}