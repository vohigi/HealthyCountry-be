using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Interfaces.Services;
using HealthyCountry.RTC.Models.Chats;
using HealthyCountry.RTC.Models.Chats.Requests;
using HealthyCountry.Utilities;
using HealthyCountry.Utilities.StringLocalizer;
using Microsoft.Extensions.Localization;

namespace HealthyCountry.RTC.Services;

public class ChatsService : IChatsService
{
    private readonly IChatsRepository _chatsRepository;
    private readonly IMapper _mapper;
    // private readonly IEventsServiceProvider _eventsServiceProvider;
    private readonly IStringLocalizer _localizer;
    //private readonly IChatNotificationQueueRepository _notificationQueue;

    public ChatsService(
        IChatsRepository chatsRepository, 
        IMapper mapper, 
        IStringLocalizerFactory localizerFactory)
        // IEventsServiceProvider eventsServiceProvider)
        //IChatNotificationQueueRepository notificationQueue)
    {
        _chatsRepository = chatsRepository;
        _mapper = mapper;
        // _eventsServiceProvider = eventsServiceProvider;
        //_notificationQueue = notificationQueue;
        _localizer = localizerFactory.Create(typeof(ValidationMessages));
    }

    public async ValueTask<ServiceResponse<ChatModel, ValidationResult>> GetChatAsync(Guid id,
        ParticipantModel requester)
    {
        var chat = await _chatsRepository.GetOneChatAsync(id);

        if (chat is null)
        {
            return new ServiceResponse<ChatModel, ValidationResult>(
                new ValidationResult()
                {
                    Errors = { new ValidationFailure("chat", _localizer.Localize(ValidationMessages.EntityNotFound)) }
                },
                ServiceResponseStatuses.NotFound);
        }

        if (!chat.Participants.Select(s => s.RelativeUserId).Contains(requester.RelativeUserId))
        {
            return new ServiceResponse<ChatModel, ValidationResult>(
                new ValidationResult()
                    { Errors = { new ValidationFailure("", _localizer.Localize(ValidationMessages.AccessDenied)) } },
                ServiceResponseStatuses.Forbidden);
        }

        return new ServiceResponse<ChatModel, ValidationResult>(chat);
    }

    public async ValueTask<ServiceResponse<ChatModel, ValidationResult>> GetChatAsync(string eventId,
        ParticipantModel requester)
    {
        if (string.IsNullOrEmpty(eventId))
        {
            return new ServiceResponse<ChatModel, ValidationResult>(
                new ValidationResult()
                    { Errors = { new ValidationFailure("eventId", _localizer.Localize(ValidationMessages.Required)) } },
                ServiceResponseStatuses.ValidationFailed);
        }

        var chat = await _chatsRepository.GetOneEventChatAsync(eventId);

        if (chat is null)
        {
            return new ServiceResponse<ChatModel, ValidationResult>(
                new ValidationResult()
                {
                    Errors = { new ValidationFailure("chat", _localizer.Localize(ValidationMessages.EntityNotFound)) }
                },
                ServiceResponseStatuses.NotFound);
        }

        if (!chat.Participants.Select(s => s.RelativeUserId).Contains(requester.RelativeUserId))
        {
            return new ServiceResponse<ChatModel, ValidationResult>(
                new ValidationResult()
                    { Errors = { new ValidationFailure("", _localizer.Localize(ValidationMessages.AccessDenied)) } },
                ServiceResponseStatuses.Forbidden);
        }

        return new ServiceResponse<ChatModel, ValidationResult>(chat);
    }

    public async ValueTask<ServiceResponse<ChatModel, ValidationResult>> GetOrCreateChatAsync(string eventId,
        ParticipantModel requester)
    {
        if (string.IsNullOrEmpty(eventId))
        {
            return new ServiceResponse<ChatModel, ValidationResult>(
                new ValidationResult()
                    { Errors = { new ValidationFailure("eventId", _localizer.Localize(ValidationMessages.Required)) } },
                ServiceResponseStatuses.ValidationFailed);
        }

        var chat = await _chatsRepository.GetOneEventChatAsync(eventId);

        if (chat is not null)
        {
            if (!chat.Participants.Select(s => s.RelativeUserId).Contains(requester.RelativeUserId))
            {
                return new ServiceResponse<ChatModel, ValidationResult>(
                    new ValidationResult()
                    {
                        Errors = { new ValidationFailure("", _localizer.Localize(ValidationMessages.AccessDenied)) }
                    },
                    ServiceResponseStatuses.Forbidden);
            }

            return new ServiceResponse<ChatModel, ValidationResult>(chat);
        }

        // var eventRequest = await _eventsServiceProvider.GetOneAsync(eventId);
        //
        // if (!eventRequest.IsSuccess)
        // {
        //     return new ServiceResponse<ChatModel, ValidationResult>(eventRequest.Errors,
        //         eventRequest.Status);
        // }
        //
        // if (eventRequest.Result.PatientCreator != requester.RelativeUserId &&
        //     eventRequest.Result.PatientId != requester.RelativeUserId &&
        //     eventRequest.Result.ResourceId != requester.RelativeUserId)
        // {
        //     return new ServiceResponse<ChatModel, ValidationResult>(
        //         new ValidationResult()
        //             { Errors = { new ValidationFailure("", _localizer.Localize(ValidationMessages.AccessDenied)) } },
        //         ServiceResponseStatuses.Forbidden);
        // }

        //TODO GET appointment data
        // var participants = new List<ExtendedParticipantModel>()
        // {
        //     new()
        //     {
        //         ResourceId = eventRequest.Result.ResourceId,
        //         UserId = eventRequest.Result.ResourceUserId,
        //         Role = Roles.Doctor
        //     },
        //     new()
        //     {
        //         UserId = eventRequest.Result.PatientId,
        //         Role = Roles.Patient
        //     },
        // };
        //
        // if (!string.IsNullOrEmpty(eventRequest.Result.PatientCreator) &&
        //     eventRequest.Result.PatientId != eventRequest.Result.PatientCreator)
        // {
        //     participants.Add(new ExtendedParticipantModel
        //     {
        //         UserId = eventRequest.Result.PatientCreator,
        //         Role = Roles.LinkedPatient
        //     });
        // }

        chat = new ChatModel
        {
            ContextType = ContextTypes.Event,
            ContextId = eventId,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            Participants = new List<ExtendedParticipantModel>(),
            MessagesCount = 0
        };
        await _chatsRepository.CreateChatAsync(chat);

        return new ServiceResponse<ChatModel, ValidationResult>(chat);
    }

    public async ValueTask<ServiceResponse<(bool hasNext, List<MessageModel> data), ValidationResult>> GetMessages(
        GetMessagesRequest request)
    {
        // var validationResult = new GetMessagesRequestValidator(_localizer).Validate(request);
        // if (!validationResult.IsValid)
        // {
        //     return new ServiceResponse<(bool hasNext, List<MessageModel> data), ValidationResult>(validationResult,
        //         ServiceResponseStatuses.ValidationFailed);
        // }

        var result = await _chatsRepository.GetMessagesAsync(request);
        return new ServiceResponse<(bool hasNext, List<MessageModel>), ValidationResult>(result);
    }

    public async ValueTask<ServiceResponse<MessageModel, ValidationResult>> SaveMessageAsync(SaveMessageRequest request)
    {
        // var validationResult = new SaveMessageRequestValidator(_localizer).Validate(request);
        // if (!validationResult.IsValid)
        // {
        //     return new ServiceResponse<MessageModel, ValidationResult>(validationResult,
        //         ServiceResponseStatuses.ValidationFailed);
        // }

        var chat = await _chatsRepository.GetOneChatAsync(request.ChatId);
        if (chat is null)
        {
            return new ServiceResponse<MessageModel, ValidationResult>(
                new ValidationResult()
                {
                    Errors = { new ValidationFailure("chat", _localizer.Localize(ValidationMessages.EntityNotFound)) }
                },
                ServiceResponseStatuses.NotFound);
        }

        // var eventRequest = await _eventsServiceProvider.GetOneAsync(chat.ContextId);
        // if (!eventRequest.IsSuccess)
        // {
        //     return new ServiceResponse<MessageModel, ValidationResult>(eventRequest.Errors,
        //         eventRequest.Status);
        // }
        //
        // if (DateTime.TryParse((string) eventRequest.Result.DateBegin, out var eventDate) && eventDate.Date < DateTime.Today)
        // {
        //     return new ServiceResponse<MessageModel, ValidationResult>(
        //         new ValidationResult()
        //             { Errors = { new ValidationFailure("", _localizer.Localize(ValidationMessages.AccessDenied)) } },
        //         ServiceResponseStatuses.Forbidden);
        // }

        var requestHasAttachments = request.Attachments?.Any() ?? false;
        var creatorParticipant =
            chat.Participants.FirstOrDefault(x => x.RelativeUserId == request.CreatedBy.RelativeUserId);

        if (creatorParticipant is null)
        {
            return new ServiceResponse<MessageModel, ValidationResult>(
                new ValidationResult()
                    { Errors = { new ValidationFailure("", _localizer.Localize(ValidationMessages.AccessDenied)) } },
                ServiceResponseStatuses.Forbidden);
        }

        if (creatorParticipant.SentMessagesCount >= 1000)
        {
            return new ServiceResponse<MessageModel, ValidationResult>(
                new ValidationResult()
                {
                    Errors =
                    {
                        new ValidationFailure("",
                            _localizer.Localize(ValidationMessages.MaxMessagesCountExceeded)
                                .Replace(ValidationMessages.LocalizationComparisonAttribute, "1000"))
                    }
                },
                ServiceResponseStatuses.ValidationFailed);
        }
        
        if (requestHasAttachments && creatorParticipant.SentAttachmentsCount >= 15)
        {
            return new ServiceResponse<MessageModel, ValidationResult>(
                new ValidationResult()
                {
                    Errors =
                    {
                        new ValidationFailure("",
                            _localizer.Localize(ValidationMessages.MaxAttachmentsCountExceeded)
                            .Replace(ValidationMessages.LocalizationComparisonAttribute, "15"))
                    }
                },
                ServiceResponseStatuses.ValidationFailed);
        }


        var message = new MessageModel()
        {
            Attachments = request.Attachments,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            CreatedBy = request.CreatedBy,
            ReadBy = new HashSet<string>() { request.CreatedBy.RelativeUserId },
            Text = request.Text,
            ChatId = chat.Id
        };

        await _chatsRepository.SaveMessageAsync(message);
        await _chatsRepository.IncrementCountersAsync(chat.Id, creatorParticipant, true, true,
            requestHasAttachments);
        foreach (var participant in chat.Participants.Where(x => x.RelativeUserId != creatorParticipant.RelativeUserId))
        {
            var delayedTill = participant.Role == Roles.Doctor
                ? DateTime.Now.AddMinutes(5)
                : DateTime.Now.AddSeconds(20);

            // await _notificationQueue.CreateAsync(new ChatNotificationTask(message.Id, chat.Id,
            //     participant.RelativeUserId,
            //     (byte)participant.Role, delayedTill, chat.ContextId));
        }
        //TODO transaction

        return new ServiceResponse<MessageModel, ValidationResult>(message);
    }

    public async ValueTask<ServiceResponse<MessageModel, ValidationResult>> MarkAsReadAsync(MarkAsReadRequest request)
    {
        // ReSharper disable once MethodHasAsyncOverload
        // var validationResult = new MarkAsReadRequestValidator(_localizer).Validate(request);
        // if (!validationResult.IsValid)
        // {
        //     return new ServiceResponse<MessageModel, ValidationResult>(validationResult,
        //         ServiceResponseStatuses.ValidationFailed);
        // }

        var message = await _chatsRepository.GetOneMessageAsync(request.MessageId);
        if (message is null)
        {
            return new ServiceResponse<MessageModel, ValidationResult>(new ValidationResult()
            {
                Errors = { new ValidationFailure("message", _localizer.Localize(ValidationMessages.EntityNotFound)) }
            }, ServiceResponseStatuses.NotFound);
        }

        if (message.ChatId != request.ChatId)
        {
            return new ServiceResponse<MessageModel, ValidationResult>(new ValidationResult()
            {
                Errors = { new ValidationFailure("message", _localizer.Localize(ValidationMessages.EntityNotFound)) }
            }, ServiceResponseStatuses.NotFound);
        }

        if (message.ReadBy.Contains(request.ReadBy.RelativeUserId))
        {
            //await _notificationQueue.CompleteForMessageAsync(message.Id, request.ReadBy.RelativeUserId);
            message.IsReadByRequester = true;
            return new ServiceResponse<MessageModel, ValidationResult>(message);
        }

        message.ReadBy.Add(request.ReadBy.RelativeUserId);
        await _chatsRepository.UpdateMessageAsync(message);
        await _chatsRepository.IncrementCountersAsync(request.ChatId, request.ReadBy, false, true, false);
        //await _notificationQueue.CompleteForMessageAsync(message.Id, request.ReadBy.RelativeUserId);
        message.IsReadByRequester = true;
        return new ServiceResponse<MessageModel, ValidationResult>(message);
    }

    public async ValueTask<ServiceResponse<Dictionary<string, UserEventChatCounters>, ValidationResult>>
        GetUserEventsCountersAsync(GetUserEventsCounters request)
    {
        var chats = (await _chatsRepository.GetEventChatsAsync(request.EventIds))
            .ToDictionary(x => x.ContextId, x => x);
        var result = new Dictionary<string, UserEventChatCounters>();
        foreach (var eventId in request.EventIds)
        {
            if (chats.TryGetValue(eventId, out var chat))
            {
                var requesterParticipant =
                    chat.Participants.FirstOrDefault(x => x.RelativeUserId == request.RelativeUserId);
                result.Add(eventId, new UserEventChatCounters()
                {
                    ChatExists = true,
                    ChatId = chat.Id,
                    EventId = eventId,
                    MessagesCount = chat.MessagesCount,
                    ReadMessagesCount = requesterParticipant?.ReadMessagesCount ?? 0,
                    SentMessagesCount = requesterParticipant?.SentMessagesCount ?? 0
                });
                continue;
            }
            
            result.Add(eventId, new UserEventChatCounters(eventId));
        }

        return new ServiceResponse<Dictionary<string, UserEventChatCounters>, ValidationResult>(result);
    }
}