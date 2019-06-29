namespace MyCorp.CityApi.Services.Countries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Services.Countries;

    /// <summary>
    /// Handles interaction with the REST Countries API
    /// </summary>
    public interface ICountryApiClient
    {
        Task<ICollection<Country>> GetCountriesAsync(string countryName);
    }
}