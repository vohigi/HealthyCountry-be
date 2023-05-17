using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.RTC.Models.Chats;
using HealthyCountry.RTC.Models.Chats.Requests;
using HealthyCountry.Utilities;

namespace HealthyCountry.RTC.Interfaces.Services;

public interface IChatsService
{
    ValueTask<ServiceResponse<ChatModel, ValidationResult>> GetChatAsync(Guid id,
        ParticipantModel requester);
    ValueTask<ServiceResponse<ChatModel, ValidationResult>> GetChatAsync(string eventId,
        ParticipantModel requester);
    ValueTask<ServiceResponse<ChatModel, ValidationResult>> GetOrCreateChatAsync(string eventId, ParticipantModel requester);
    ValueTask<ServiceResponse<(bool hasNext, List<MessageModel> data), ValidationResult>> GetMessages(GetMessagesRequest request);
    ValueTask<ServiceResponse<MessageModel, ValidationResult>> SaveMessageAsync(SaveMessageRequest request);
    ValueTask<ServiceResponse<MessageModel, ValidationResult>> MarkAsReadAsync(MarkAsReadRequest request);
    ValueTask<ServiceResponse<Dictionary<string, UserEventChatCounters>, ValidationResult>> GetUserEventsCountersAsync(
        GetUserEventsCounters request);
}