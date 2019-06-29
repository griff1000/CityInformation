namespace MyCorp.CityApi.Services.Countries
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Options;
    using Models.Services.Countries;
    using Newtonsoft.Json;
    using Options;

    public class CountryApiClient : ICountryApiClient
    {
        private readonly HttpClient _client;
        private readonly IOptionsMonitor<AppSettingsOptions> _appSettings;

        public CountryApiClient(HttpClient client, IOptionsMonitor<AppSettingsOptions> appSettings)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _appSettings = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task<ICollection<Country>> GetCountriesAsync(string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) throw new ArgumentNullException(nameof(countryName));

            //TODO: Ideally I'd use something like WebUtility.UrlEncode here - but that doesn't return the expected encoding
            var encodedCountryName = countryName.Replace(" ", "%20");

            var apiUrl = string.Format(_appSettings.CurrentValue.CountryApiUrl, encodedCountryName);

            var response = await _client.GetAsync(apiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ICollection<Country>>(responseContent);

            throw new Exception($"Failed to call Weather API.  Status code: {response.StatusCode}");
        }
    }
}