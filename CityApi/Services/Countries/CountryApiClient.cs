namespace MyCorp.CityApi.Services.Countries
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Models.Services.Countries;
    using Newtonsoft.Json;

    public class CountryApiClient : ICountryApiClient
    {
        private readonly HttpClient _client;

        public CountryApiClient(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task<ICollection<Country>> GetCountriesAsync(string countryName)
        {
            if (string.IsNullOrEmpty(countryName)) throw new ArgumentNullException(nameof(countryName));

            //TODO: Ideally I'd use something like WebUtility.UrlEncode here - but that doesn't return the expected encoding
            var encodedCountryName = countryName.Replace(" ", "%20");

            var apiUrl = $"https://restcountries.eu/rest/v2/name/{encodedCountryName}";

            var response = await _client.GetAsync(apiUrl);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                return JsonConvert.DeserializeObject<ICollection<Country>>(responseContent);

            throw new Exception($"Failed to call Weather API.  Status code: {response.StatusCode}");
        }
    }
}