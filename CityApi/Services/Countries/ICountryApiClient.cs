namespace MyCorp.CityApi.Services.Countries
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Services.Countries;

    public interface ICountryApiClient
    {
        Task<ICollection<Country>> GetCountriesAsync(string countryName);
    }
}