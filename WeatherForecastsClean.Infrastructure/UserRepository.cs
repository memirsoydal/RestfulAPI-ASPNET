using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Core;

namespace WeatherForecastsClean.Infrastructure;

public class UserRepository : IUserRepository
{
    private readonly IMongoCollection<User> _mongoCollection;

    public UserRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _mongoCollection = mongoDatabase.GetCollection<User>(databaseSettings.Value.UserCollectionName);
    }

    public async Task<User?> GetUserAsync(string id)
    {
        return await _mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> LoginUserAsync(string username, string password)
    {
        return await _mongoCollection.Find(x => x.Username == username && x.Password == password).FirstOrDefaultAsync();
    }
}