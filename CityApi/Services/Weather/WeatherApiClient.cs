namespace MyCorp.CityApi.Services.Weather
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Models.Services.Weather;
    using Newtonsoft.Json;
    using Options;

    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _client;
        private readonly IOptionsMonitor<AppSettingsOptions> _appSettings;

        public WeatherApiClient(HttpClient client, IOptionsMonitor<AppSettingsOptions> appSettings)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }
        public async Task<Envelope> GetWeatherForCityAsync(string city, string appId)
        {
            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
            if (string.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));

            city = city.Replace(" ", "%20");

            var apiUrl = string.Format(_appSettings.CurrentValue.OpenweatherApiUrl, city, appId);

            var response = await _client.GetAsync(apiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<Envelope>(responseContent);

            throw new Exception($"Failed to call Weather API.  Status code: {response.StatusCode}");
        }
    }
}