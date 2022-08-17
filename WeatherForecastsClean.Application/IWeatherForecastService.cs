using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.Application
{
    public interface IWeatherForecastService
    {
        Task<WeatherForecast> ProcessFTemperatureAsync(WeatherForecast newForecast);
    }
}
