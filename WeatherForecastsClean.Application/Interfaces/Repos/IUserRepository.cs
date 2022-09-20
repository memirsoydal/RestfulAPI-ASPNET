using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.Application.Interfaces.Repos;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string id);
    Task<User?> LoginUserAsync(string username, string password);
}