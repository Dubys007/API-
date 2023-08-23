using Sq016FirstApi.Data.Entities;

namespace Sq016FirstApi.Data.Repositories
{
    public class WeatherForecastRepository : IWeatherForecastRepository
    {
        private readonly WeatherForecastDbContext _context;
        public WeatherForecastRepository(WeatherForecastDbContext context) 
        {
            _context = context;
        }

        public bool Add(WeatherForecast weatherForecast)
        {
            _context.Add(weatherForecast);
            _context.SaveChanges();
            return true;
        }

        public bool Delete(WeatherForecast weatherForecast)
        {
            _context.Remove(weatherForecast);
            _context.SaveChanges();
            return true;
        }

        public bool Update(WeatherForecast weatherForecast)
        {
            _context.Update(weatherForecast);
            _context.SaveChanges();
            return true;
        }

        public WeatherForecast WeatherForecastGet(int id)
        {
            var result =  _context.WeatherForecasts.FirstOrDefault(x => x.Id == id);
            if(result != null)
                return result;

            return new WeatherForecast();
        }

        public List<WeatherForecast> WeatherForecastListAll()
        {
            return _context.WeatherForecasts.ToList(); ;
        }
    }
}
