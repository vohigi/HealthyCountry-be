using HealthyCountry.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities;

[BsonIgnoreExtraElements]
public class UserConnectionEntity
{
    [BsonElement("cid")]
    public string ConnectionId { get; set; }
    [BsonElement("r")]
    public UserRoles Role { get; set; }
    [BsonElement("ruid")]
    public string RelativeUserId { get; set; }
}