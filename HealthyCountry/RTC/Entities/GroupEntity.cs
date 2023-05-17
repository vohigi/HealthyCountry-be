using System;
using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities;

[BsonIgnoreExtraElements]
public class GroupEntity
{
    [BsonId]
    public string Id { get; set; }

    [BsonElement("uc")]
    public IEnumerable<UserConnectionEntity> UserConnections { get; set; }

    [BsonElement("uat")]
    public DateTime LastUpdatedAt { get; set; }
}