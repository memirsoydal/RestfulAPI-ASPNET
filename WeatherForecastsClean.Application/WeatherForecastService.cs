using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.Application
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly IWeatherForecastRepository _weatherForecastRepository;
        public WeatherForecastService(IWeatherForecastRepository weatherForecastRepository)
        {
            _weatherForecastRepository = weatherForecastRepository;
        }
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
}
