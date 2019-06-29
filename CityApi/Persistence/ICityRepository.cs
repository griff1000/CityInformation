namespace MyCorp.CityApi.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models.Database;

    /// <summary>
    /// Handles all persistence-related data activies i.e. storing and retrieving data
    /// </summary>
    public interface ICityRepository
    {
        Task<ICollection<City>> GetCitiesAsync(string cityName);
        Task<City> GetCityAsync(Guid cityId);
        void DeleteCity(City city);
        Task CreateCityAsync(City city);
        void UpdateCity(City city);
        Task<bool> SaveAsync();
    }
}