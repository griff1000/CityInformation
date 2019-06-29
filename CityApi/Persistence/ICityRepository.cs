namespace MyCorp.CityApi.Persistence
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Database;

    public interface ICityRepository
    {
        IQueryable<City> GetCities(string cityName);
        Task<City> GetCityAsync(Guid cityId);
        void DeleteCity(City city);
        Task CreateCityAsync(City city);
        void UpdateCity(City city);
        Task<bool> SaveAsync();
    }
}