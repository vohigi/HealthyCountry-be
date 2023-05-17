
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities.Chats;

[BsonIgnoreExtraElements]
public class AttachmentEntity
{
    [BsonElement("did")]
    public string DocumentId { get; set; }
    
    [BsonElement("fid")]
    public string FileId { get; set; }
    
    [BsonElement("fn")]
    public string FileName { get; set; }
}