using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherForecastsClean.Core;

public class User : BaseEntity
{
    // [BsonId]
    // [BsonRepresentation(BsonType.ObjectId)]
    // public string? Id { get; set; }

    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string Role { get; set; } = null!;
    public DateTime? CreatedDate { get; set; }
}