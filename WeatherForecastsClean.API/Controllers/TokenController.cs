using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Application.Interfaces.Repos;
using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TokenController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _repository;

    public TokenController(IConfiguration configuration, IUserRepository repository)
    {
        _configuration = configuration;
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Post(UserDto userData)
    {
        if (userData.Username == null || userData.Password == null) return BadRequest();
        var user = await _repository.LoginUserAsync(userData.Username, userData.Password);
        if (user == null) return BadRequest("Invalid credentials");
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim("UserId", user.Id!),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.UtcNow.AddMinutes(10),
            signingCredentials: signIn);

        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}