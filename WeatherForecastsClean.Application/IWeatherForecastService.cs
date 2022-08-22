using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.Application;

public interface IWeatherForecastService
{
    Task<WeatherForecast> ProcessFTemperatureAsync(WeatherForecast newForecast);
}