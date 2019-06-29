﻿namespace MyCorp.CityApi.Persistence
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Database;
    using Models.Database;

    public class CityRepository : ICityRepository
    {
        private readonly ApiContext _context;

        public CityRepository(ApiContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task CreateCityAsync(City city)
        {
            city.CityId = Guid.NewGuid();
            await _context.Cities.AddAsync(city);
        }

        public void DeleteCity(City cityToDelete)
        {
            _context.Cities.Remove(cityToDelete);
        }

        public IQueryable<City> GetCities(string cityName)
        {
            var response = _context.Cities;
            if (cityName != "*")
            {
                return response.Where(city => city.Name.StartsWith(cityName));
            }

            return response;
        }

        public Task<City> GetCityAsync(Guid cityId)
        {
            return _context.Cities.FindAsync(cityId);
        }

        public async Task<bool> SaveAsync()
        {
            var result = await _context.SaveChangesAsync();
            return result >= 0;
        }

        public void UpdateCity(City city)
        {
            return;
        }
    }
}