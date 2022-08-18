﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WeatherForecastsClean.Core;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public DateTime? CreatedDate { get; set; }
    
}