using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyCountry.RTC.Models.Chats;
using HealthyCountry.RTC.Models.Chats.Requests;

namespace HealthyCountry.RTC.Interfaces.Repositories;

public interface IChatsRepository
{
    Task<ChatModel> GetOneChatAsync(Guid id);
    
    Task<ChatModel> GetOneEventChatAsync(string eventId);
    Task<IEnumerable<ChatModel>> GetEventChatsAsync(IEnumerable<string> eventIds);
    
    Task CreateChatAsync(ChatModel model);

    Task IncrementCountersAsync(Guid chatId, ParticipantModel participant, bool incrementSentMessages,
        bool incrementReadMessages, bool incrementSentAttachments);
    
    Task<(bool hasNext, List<MessageModel>)> GetMessagesAsync(GetMessagesRequest request);
    Task<MessageModel> GetOneMessageAsync(Guid id);
    Task SaveMessageAsync(MessageModel model);
    Task UpdateMessageAsync(MessageModel model);
}