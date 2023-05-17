using System;
using System.Collections.Generic;

namespace HealthyCountry.RTC.Models.Chats;

public class ChatModel
{
    public Guid Id { get; set; } = Guid.NewGuid();
    
    public string ContextId { get; set; }
    
    public ContextTypes ContextType { get; set; }
    
    public IEnumerable<ExtendedParticipantModel> Participants { get; set; }

    public long MessagesCount { get; set; } = 0;
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime UpdatedAt { get; set; }
}

public enum ContextTypes : byte
{
    Event = 0
}