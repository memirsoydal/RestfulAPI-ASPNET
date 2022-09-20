using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Application.Interfaces.Repos;
using WeatherForecastsClean.Application.Interfaces.Services;
using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.API.Controllers;

[Authorize(Roles = "Admin, User")]
[ApiController]
[Route("[controller]/[action]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IWeatherForecastRepository _repository;
    private readonly IWeatherForecastService _service;

    public WeatherForecastController(ILoggerFactory factory, IWeatherForecastService service,
        IWeatherForecastRepository forecastRepository)
    {
        _logger = factory.CreateLogger("WeatherForecastController");
        _service = service;
        _repository = forecastRepository;
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<List<WeatherForecast>> Get()
    {
        _logger.LogInformation("All WeatherForecast has been brought {Time}", DateTime.Now);
        return await _repository.GetForecastsAsync();
    }

    [Authorize(Roles = "Admin, User")]
    [HttpGet(Name = "GetWeatherForecast/{id}")]
    public async Task<ActionResult<WeatherForecast>> GetById(string id)
    {
        var forecast = await _repository.GetForecastsAsync(id);
        if (forecast is null)
        {
            _logger.LogWarning("WeatherForecast has not been found {Time}", DateTime.Now);
            return NotFound();
        }

        _logger.LogInformation("WeatherForecast has been brought {Time}", DateTime.Now);
        return forecast;
    }
    [Authorize(Roles = "Admin")]
    [HttpPost(Name = "PostWeatherForecast")]
    public async Task<IActionResult> Post(WeatherForecast newForecast)
    {
        newForecast = await _service.ProcessFTemperatureAsync(newForecast);

        await _repository.CreateForecastsAsync(newForecast);
        _logger.LogInformation("WeatherForecast has been created {NewForecast} {Time}", newForecast.Id, DateTime.Now);
        return CreatedAtAction(nameof(Get), new { id = newForecast.Id }, newForecast);
    }
    
    [Authorize(Roles = "Admin")]
    [HttpPut(Name = "PutWeatherForecast")]
    public async Task<IActionResult> Put(string id, WeatherForecast updatedForecast)
    {
        var forecast = await _repository.GetForecastsAsync(id);

        if (forecast is null)
            return NotFound();

        updatedForecast.TemperatureF = _service.ProcessFTemperatureAsync(forecast).Result.TemperatureF;
        updatedForecast.Id = forecast.Id;
        await _repository.ReplaceForecastsAsync(id, updatedForecast);
        _logger.LogInformation("WeatherForecast has been updated {Time}", DateTime.Now);
        return NoContent();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete(Name = "DeleteWeatherForecast")]
    public async Task<IActionResult> Delete(string id)
    {
        var forecast = await _repository.GetForecastsAsync(id);

        if (forecast is null)
            return NotFound();

        await _repository.DeleteForecastAsync(id);
        _logger.LogInformation("WeatherForecast has been deleted {Time}", DateTime.Now);
        return NoContent();
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete(Name = "DeleteAllWeatherForecast")]
    public async Task<IActionResult> DeleteAll()
    {
        await _repository.DeleteAllForecastsAsync();
        _logger.LogInformation("All WeatherForecast has been deleted {Time}", DateTime.Now);
        return NoContent();
    }

    [AllowAnonymous]
    [HttpPost(Name = "SearchWeatherForecast")]
    public async Task<ActionResult> Search(string summary)
    {
        var forecasts = await _repository.SearchForecastAsync(summary);
        if (forecasts == null)
        {
            _logger.LogWarning("WeatherForecast has not been returned {Time}", DateTime.Now);
            return BadRequest("Summary is less than 3 characters.");
        }

        _logger.LogInformation("WeatherForecast has been returned {Time}", DateTime.Now);
        return Ok(forecasts);
    }
}