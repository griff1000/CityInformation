namespace MyCorp.CityApi.Persistence
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Database;
    using Microsoft.EntityFrameworkCore;
    using Models.Database;

    /// <summary>
    /// Handles all persistence-related data activities i.e. storing and retrieving data
    /// </summary>
    public class CityRepository : ICityRepository
    {
        private readonly ApiContext _context;

        public CityRepository(ApiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Saves a new city to the database context and generates a new CityId for it.  Does not persist the data until SaveAsync is called.
        /// </summary>
        /// <param name="city">The City object to store</param>
        public async Task CreateCityAsync(City city)
        {
            city.CityId = Guid.NewGuid();
            await _context.Cities.AddAsync(city);
        }

        /// <summary>
        /// Removes a city from the database context.  Does not persist the change until SaveAsync is called.
        /// </summary>
        /// <param name="cityToDelete">The City object to delete</param>
        public void DeleteCity(City cityToDelete)
        {
            _context.Cities.Remove(cityToDelete);
        }

        /// <summary>
        /// Gets all cities which start with the city name passed in.
        /// </summary>
        /// <param name="cityName">The start of, or an entire, city name OR the wildcard character "*" for all cities</param>
        /// <returns>Collection of matching City records</returns>
        public async Task<ICollection<City>> GetCitiesAsync(string cityName)
        {
            var response = _context.Cities;
            if (cityName != "*")
            {
                return await response.Where(city => city.Name.StartsWith(cityName)).ToListAsync();

            }

            return await response.ToListAsync();
        }

        /// <summary>
        /// Gets a specific City by Id
        /// </summary>
        /// <param name="cityId">The Id of the city to get</param>
        /// <returns>The City record for that Id</returns>
        public Task<City> GetCityAsync(Guid cityId)
        {
            return _context.Cities.FindAsync(cityId);
        }

        /// <summary>
        /// Saves changes in the context to the underlying database
        /// </summary>
        /// <returns>True if no errors were encountered</returns>
        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result >= 0;
        }


        /// <summary>
        /// Updates the given City - does not persist changes till the SaveAsync method is called
        /// </summary>
        /// <param name="city">The city to update</param>
        /// <remarks>This particular implementation of the database context doesnt need to do anything
        /// for an update - the City object passed in should have been attached to the context and will have
        /// been marked for update when changed.  However, other persistence mechanisms might need to do something
        /// at this point so still worth calling it from the client application</remarks>
        public void UpdateCity(City city)
        {
            //Do nothing
        }
    }
}