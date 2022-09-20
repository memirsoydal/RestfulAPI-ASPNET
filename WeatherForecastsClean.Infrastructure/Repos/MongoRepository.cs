﻿using Microsoft.Extensions.Options;
using MongoDB.Driver;
using WeatherForecastsClean.Application;
using WeatherForecastsClean.Application.Interfaces.Repos;
using WeatherForecastsClean.Core.Models;

namespace WeatherForecastsClean.Infrastructure.Repos;

public class MongoRepository <T> : IMongoRepository<T> where T: BaseEntity
{
    private readonly IMongoCollection<T> _mongoCollection;

    public MongoRepository(IOptions<DatabaseSettings> databaseSettings)
    {
        var mongoClient = new MongoClient(databaseSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(databaseSettings.Value.DatabaseName);
        _mongoCollection = mongoDatabase.GetCollection<T>(typeof(T).Name);
    }
    
    public Task<IEnumerable<T>> GetAllAsync()
    {
        return Task.FromResult(_mongoCollection.Find(_ => true).ToEnumerable());
    }

    public async Task<T?> GetAsync(string id)
    {
        return await _mongoCollection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(T entity)
    {
        await _mongoCollection.InsertOneAsync(entity);
    }

    public async Task CreateManyAsync(IEnumerable<T> entities)
    {
        await _mongoCollection.InsertManyAsync(entities);
    }

    public async Task ReplaceAsync(string id, T entity)
    {
        await _mongoCollection.ReplaceOneAsync(x => x.Id == id, entity);
    }

    public async Task DeleteAsync(string id)
    {
        await _mongoCollection.DeleteOneAsync(id);
    }

    public async Task DeleteAllAsync()
    {
        await _mongoCollection.DeleteManyAsync(_ => true);
    }
}