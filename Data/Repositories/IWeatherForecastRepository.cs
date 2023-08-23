using Sq016FirstApi.Data.Entities;

namespace Sq016FirstApi.Data.Repositories
{
    public interface IWeatherForecastRepository
    {
        bool Add(WeatherForecast weatherForecast);
        bool Update(WeatherForecast weatherForecast);
        bool Delete(WeatherForecast weatherForecast);
        WeatherForecast WeatherForecastGet(int id);
        List<WeatherForecast> WeatherForecastListAll();
    }
}
