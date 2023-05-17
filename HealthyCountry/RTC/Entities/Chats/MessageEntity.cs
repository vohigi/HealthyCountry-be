using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities.Chats;

[BsonIgnoreExtraElements, Table("rtc_messages")]
public class MessageEntity
{
    [BsonId, BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonElement("cid"), BsonRepresentation(BsonType.String)]
    public Guid ChatId { get; set; }
    
    [BsonElement("t")]
    public string Text { get; set; }
    
    [BsonElement("a")]
    public IEnumerable<AttachmentEntity> Attachments { get; set; }
    
    [BsonElement("rby")]
    public HashSet<string> ReadBy { get; set; }
    
    [BsonElement("cby")]
    public ParticipantEntity CreatedBy { get; set; }

    [BsonElement("cat")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("uat")]
    public DateTime UpdatedAt { get; set; }
}