using System;

namespace HealthyCountry.RTC.Models.Chats;

public class UserEventChatCounters
{
    public bool ChatExists { get; set; }
    public Guid? ChatId { get; set; }
    public string EventId { get; set; }
    public long MessagesCount { get; set; }
    public long ReadMessagesCount { get; set; }
    public int SentMessagesCount { get; set; }

    public UserEventChatCounters()
    {
        
    }

    public UserEventChatCounters(string eventId)
    {
        EventId = eventId;
        ChatExists = false;
        MessagesCount = 0;
        ReadMessagesCount = 0;
        SentMessagesCount = 0;
    }
}