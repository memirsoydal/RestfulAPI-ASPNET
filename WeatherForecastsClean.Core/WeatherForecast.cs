using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherForecastsClean.Core;

public class WeatherForecast
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public DateTime Date { get; set; }
    public int TemperatureC { get; set; }
    public int? TemperatureF { get; set; }
    public string? Summary { get; set; }
}