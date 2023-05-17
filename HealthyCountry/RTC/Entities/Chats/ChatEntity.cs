using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities.Chats;

[BsonIgnoreExtraElements, Table("rtc_chats")]
public class ChatEntity
{
    [BsonId, BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }
    
    [BsonElement("cid")]
    public string ContextId { get; set; }
    
    [BsonElement("ct")]
    public byte ContextType { get; set; }
    
    [BsonElement("p")]
    public IEnumerable<ExtendedParticipantEntity> Participants { get; set; }

    [BsonElement("mc")]
    public long MessagesCount { get; set; } = 0;
    
    [BsonElement("cat")]
    public DateTime CreatedAt { get; set; }
    
    [BsonElement("uat")]
    public DateTime UpdatedAt { get; set; }
}