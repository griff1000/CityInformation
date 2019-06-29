namespace MyCorp.CityApi.Services.Weather
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models.Services.Weather;
    using Newtonsoft.Json;

    public class WeatherApiClient : IWeatherApiClient
    {
        private readonly HttpClient _client;

        public WeatherApiClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<Envelope> GetWeatherForCityAsync(string city, string appId)
        {
            if (string.IsNullOrEmpty(city)) throw new ArgumentNullException(nameof(city));
            if (string.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));

            city = city.Replace(" ", "%20");

            var apiUrl = $"https://api.openweathermap.org/data/2.5/weather?q={city}&APPID={appId}";

            var response = await _client.GetAsync(apiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode) return JsonConvert.DeserializeObject<Envelope>(responseContent);

            throw new Exception($"Failed to call Weather API.  Status code: {response.StatusCode}");
        }
    }
}