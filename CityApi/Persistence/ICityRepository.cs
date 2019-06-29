namespace MyCorp.CityApi.Persistence
{
    using System.Linq;
    using System.Threading.Tasks;
    using Models.Database;

    public interface ICityRepository
    {
        IQueryable<City> GetCities(string cityName);
        void DeleteCity(int cityId);
        Task CreateCityAsync(City city);
        void UpdateCity(City city);
        Task<bool> SaveAsync();
    }
}