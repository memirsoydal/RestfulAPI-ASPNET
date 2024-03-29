﻿using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.Application.Interfaces.Repos;

public interface IWeatherForecastRepository
{
    Task<List<WeatherForecast>> GetForecastsAsync();
    Task<WeatherForecast?> GetForecastsAsync(string id);
    Task CreateForecastsAsync(WeatherForecast weatherForecast);
    Task ReplaceForecastsAsync(string id, WeatherForecast weatherForecast);
    Task DeleteForecastAsync(string id);
    Task DeleteAllForecastsAsync();
    Task<List<WeatherForecast>?> SearchForecastAsync(string summary);
}