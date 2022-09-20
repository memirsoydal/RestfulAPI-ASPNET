using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Application.Interfaces.Repos;
using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.Infrastructure.Repos;

public class WeatherForecastRepository : IWeatherForecastRepository
{
    private readonly IMongoCollection<WeatherForecast> _mongoCollection;

    public WeatherForecastRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _mongoCollection = mongoDatabase.GetCollection<WeatherForecast>(databaseSettings.Value.CollectionName);
    }

    public async Task<List<WeatherForecast>> GetForecastsAsync()
    {
        return await _mongoCollection.Find(_ => true).ToListAsync();
    }

    public async Task<WeatherForecast?> GetForecastsAsync(string id)
    {
        return await _mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateForecastsAsync(WeatherForecast newForecast)
    {
        await _mongoCollection.InsertOneAsync(newForecast);
    }

    public async Task ReplaceForecastsAsync(string id, WeatherForecast replacedForecast)
    {
        await _mongoCollection.ReplaceOneAsync(x => x.Id == id, replacedForecast);
    }

    public async Task DeleteForecastAsync(string id)
    {
        await _mongoCollection.DeleteOneAsync(x => x.Id == id);
    }

    public async Task DeleteAllForecastsAsync()
    {
        await _mongoCollection.DeleteManyAsync(Builders<WeatherForecast>.Filter.Empty);
    }

    public async Task<List<WeatherForecast>?> SearchForecastAsync(string summary)
    {
        var count = summary.Length;
        if (count < 3) return null;
        var filter = Builders<WeatherForecast>.Filter.Regex("Summary", new BsonRegularExpression(summary, "i"));
        return await (await _mongoCollection.FindAsync(filter)).ToListAsync();
    }
}