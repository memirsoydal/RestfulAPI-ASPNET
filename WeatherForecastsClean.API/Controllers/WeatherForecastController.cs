using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger _logger;
        private readonly IWeatherForecastService _service;
        private readonly IWeatherForecastRepository _repository;

        public WeatherForecastController(ILoggerFactory factory, IWeatherForecastService service, IWeatherForecastRepository forecastRepository)
        {
            _logger = factory.CreateLogger("WeatherForecastController");
            _service = service;
            _repository = forecastRepository;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<List<WeatherForecast>> Get()
        { 
            _logger.LogInformation("All WeatherForecast has been brought {Time}", DateTime.Now);
            return await _repository.GetForecastsAsync();
        }

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
        
        [HttpPost(Name = "PostWeatherForecast")]
        public async Task<IActionResult> Post(WeatherForecast newForecast)
        {
            newForecast = await _service.ProcessFTemperatureAsync(newForecast);
            
            await _repository.CreateForecastsAsync(newForecast);
            _logger.LogInformation("WeatherForecast has been created {NewForecast} {Time}", newForecast.Id,DateTime.Now);
            return CreatedAtAction(nameof(Get), new { id = newForecast.Id }, newForecast);
        }
        
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

        [HttpDelete(Name = "DeleteAllWeatherForecast")]
        public async Task<IActionResult> DeleteAll()
        {
            await _repository.DeleteAllForecastsAsync();
            _logger.LogInformation("All WeatherForecast has been deleted {Time}", DateTime.Now);
            return NoContent();
        }

        [HttpPost(Name = "SearchWeatherForecast")]
        public async Task<ActionResult> Search(string summary)
        {
            var forecasts = await _repository.SearchForecastAsync(summary);
            _logger.LogInformation("WeatherForecast has been returned {Time}", DateTime.Now);
            return Ok(forecasts);
        }
    }
}