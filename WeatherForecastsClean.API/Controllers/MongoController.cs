using Microsoft.AspNetCore.Mvc;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongoController : ControllerBase
    {
        private readonly IMongoRepository<WeatherForecast> _repoForecast;
        private readonly IMongoRepository<User> _repoUser; 

        public MongoController(IMongoRepository<WeatherForecast> repoForecast, IMongoRepository<User> repoUser)
        {
            _repoForecast = repoForecast;
            _repoUser = repoUser;
        }

        [HttpGet("GetWeatherForecast/{id}")]
        public async Task<IActionResult> GetForecasts(string id)
        {
            return Ok(await _repoForecast.GetAsync(id));
        }
        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            return Ok(await _repoUser.GetAllAsync());
        }
    }
}
