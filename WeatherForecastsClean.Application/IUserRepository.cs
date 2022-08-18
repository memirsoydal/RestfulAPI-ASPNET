using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.Application;

public interface IUserRepository
{
    Task<User?> GetUserAsync(string username, string password);
}