using System;
using System.Collections.Generic;

namespace HealthyCountry.RTC.Models.Chats;

public class MessageModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public Guid ChatId { get; set; }
    
    public string Text { get; set; }
    
    public IEnumerable<AttachmentModel> Attachments { get; set; }
    
    internal HashSet<string> ReadBy { get; set; }
    
    public bool IsReadByRequester { get; set; }
    
    public ParticipantModel CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
    
}