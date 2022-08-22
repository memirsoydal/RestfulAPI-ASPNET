using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.Application;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string id);
    Task<User?> LoginUserAsync(string username, string password);
}