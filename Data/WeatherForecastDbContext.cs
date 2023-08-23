using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sq016FirstApi.Data.Entities;

namespace Sq016FirstApi.Data
{
    public class WeatherForecastDbContext : IdentityDbContext<AppUser>
    {
        public WeatherForecastDbContext(DbContextOptions<WeatherForecastDbContext> options): base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}
