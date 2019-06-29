namespace MyCorp.CityApi.Database
{
    using Microsoft.EntityFrameworkCore;
    using Models.Database;

    public class ApiContext : DbContext
    {
        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<City> Cities { get; set; }
    }
}