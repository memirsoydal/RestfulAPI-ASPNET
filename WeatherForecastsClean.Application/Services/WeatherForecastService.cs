using WeatherForecastsClean.Application.Interfaces.Services;
using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.Application.Services;

public class WeatherForecastService : IWeatherForecastService
{
    public Task<WeatherForecast> ProcessFTemperatureAsync(WeatherForecast newForecast)
    {
        try
        {
            newForecast.TemperatureF = 32 + (int)(newForecast.TemperatureC / 0.5556);

            return Task.FromResult(newForecast);
        }
        catch (Exception e)
        {
            var message = e.Message;
            throw new ArgumentNullException(message);
        }
    }
}