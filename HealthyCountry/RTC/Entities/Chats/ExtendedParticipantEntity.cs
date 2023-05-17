using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities.Chats;

[BsonIgnoreExtraElements]
public class ExtendedParticipantEntity : ParticipantEntity
{
    [BsonElement("smc")]
    public int SentMessagesCount { get; set; }
    
    [BsonElement("sac")]
    public int SentAttachmentsCount { get; set; }
    
    [BsonElement("rmc")]
    public int ReadMessagesCount { get; set; }
}