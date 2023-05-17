using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities.Chats;

[BsonIgnoreExtraElements]
public class ParticipantEntity
{
    [BsonElement("uid")]
    public string UserId { get; set; }

    [BsonElement("rid")]
    public string ResourceId { get; set; }
    
    [BsonElement("r")]
    public byte Role { get; set; }
}