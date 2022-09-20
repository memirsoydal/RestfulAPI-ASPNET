using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Application.Interfaces.Repos;
using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.API.Controllers;

[Authorize(Roles = "Admin, User")]
[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IUserRepository _repository;

    public UserController(ILoggerFactory factory, IUserRepository repository)
    {
        _logger = factory.CreateLogger("UserController");
        _repository = repository;
    }
    
    [HttpGet(Name = "GetUser")]
    public async Task<ActionResult<User>> Get()
    {
        var userId = User.FindFirstValue("UserId");
        var user = await _repository.GetUserAsync(userId);
        if (user == null) return NotFound("User not found");
        _logger.LogInformation("All user has been brought {Time}", DateTime.Now);
        return Ok(user);
    }
}