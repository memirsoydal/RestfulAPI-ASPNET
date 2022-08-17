using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.Application
{
    public interface IWeatherForecastRepository
    {
        Task<List<WeatherForecast>> GetForecastsAsync();
        Task<WeatherForecast?> GetForecastsAsync(string id);
        Task CreateForecastsAsync(WeatherForecast weatherForecast);
        Task ReplaceForecastsAsync(string id, WeatherForecast weatherForecast);
        Task DeleteForecastAsync(string id);
        Task DeleteAllForecastsAsync();
        Task<List<WeatherForecast>> SearchForecastAsync(string summary);
    }
}
