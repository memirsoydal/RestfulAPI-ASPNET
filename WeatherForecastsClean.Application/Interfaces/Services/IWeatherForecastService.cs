using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.Application.Interfaces.Services;

public interface IWeatherForecastService
{
    Task<WeatherForecast> ProcessFTemperatureAsync(WeatherForecast newForecast);
}