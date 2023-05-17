using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.Hubs.Models;
using HealthyCountry.RTC.Models;
using HealthyCountry.RTC.Models.Chats;
using HealthyCountry.RTC.Models.Chats.Requests;
using HealthyCountry.Utilities;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace HealthyCountry.Hubs;

public partial class RtcHub
{
    [HubMethodName("GetEventChat")]
    public async ValueTask<HubResponse<ChatModel>> GetEventChatAsync(string eventId)
    {
        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<ChatModel>(userRequest);
        }
        var participant = _mapper.Map<ParticipantModel>(userRequest.Result);
        var getChatRequest = await _chatsService.GetChatAsync(eventId, participant);

        return !getChatRequest.IsSuccess ? new HubResponse<ChatModel>(getChatRequest) : new HubResponse<ChatModel>(getChatRequest.Result);
    }
    
    [HubMethodName("JoinEventChat")]
    public async ValueTask<HubResponse<ChatModel>> JoinEventChatAsync(string eventId)
    {
        if (string.IsNullOrEmpty(eventId))
        {
            return new HubResponse<ChatModel>(new ServiceResponse<ValidationResult>(
                new ValidationResult() {Errors = {new ValidationFailure("eventId", "Entity not found")}},
                ServiceResponseStatuses.NotFound));
        }
        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<ChatModel>(userRequest);
        }
        var participant = _mapper.Map<ParticipantModel>(userRequest.Result);
        var getChatRequest = await _chatsService.GetOrCreateChatAsync(eventId, participant);

        if (!getChatRequest.IsSuccess)
        {
            return new HubResponse<ChatModel>(getChatRequest);
        }
        
        await _chatsGroupsService.AddUserToGroup(GetChatsGroupId(getChatRequest.Result.Id), _mapper.Map<UserModel>(userRequest.Result));
        await Groups.AddToGroupAsync(Context.ConnectionId, GetChatsGroupId(getChatRequest.Result.Id));
        await Clients.OthersInGroup(GetChatsGroupId(getChatRequest.Result.Id)).ReceiveChatEvent(new HubMessage<Dictionary<string, object>>()
        {
            MessageType = MessageTypes.Message,
            Event = "UserJoinedChat",
            Data = new Dictionary<string, object>(){{"user", participant}}
        });
        
        return new HubResponse<ChatModel>(getChatRequest.Result);
    }
    
    [HubMethodName("GetChatMessages")]
    public async ValueTask<HubResponse<IList<MessageModel>>> GetChatMessagesAsync(GetMessagesRequest request)
    {
        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<IList<MessageModel>>(userRequest);
        }
        request.Requester = _mapper.Map<ParticipantModel>(userRequest.Result);
        var getMessagesRequest = await _chatsService.GetMessages(request);
        
        if (!getMessagesRequest.IsSuccess)
        {
            return new HubResponse<IList<MessageModel>>(getMessagesRequest);
        }

        return new HubResponse<IList<MessageModel>>(getMessagesRequest.Result.data, getMessagesRequest.Result.hasNext, request.Limit, request.Skip);
    }
    
    [HubMethodName("SendChatMessage")]
    public async ValueTask<HubResponse<MessageModel>> SendMessage(SaveMessageRequest request)
    {
        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<MessageModel>(userRequest);
        }
        request.CreatedBy = _mapper.Map<ParticipantModel>(userRequest.Result);
        var saveMessageRequest = await _chatsService.SaveMessageAsync(request);
        
        if (!saveMessageRequest.IsSuccess)
        {
            return new HubResponse<MessageModel>(saveMessageRequest);
        }

        await Clients.OthersInGroup(GetChatsGroupId(request.ChatId)).ReceiveChatMessage(new HubMessage<MessageModel>()
        {
            Event = "NewChatMessage",
            Data = saveMessageRequest.Result
        });
        await SendCounterUpdatedNotification(request.ChatId, request.CreatedBy);
        
        saveMessageRequest.Result.IsReadByRequester = true;
        return new HubResponse<MessageModel>(saveMessageRequest.Result);
    }

    [HubMethodName("MarkMessageAsRead")]
    public async ValueTask<HubResponse<MessageModel>> MarkAsRead(MarkAsReadRequest request)
    {
        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<MessageModel>(userRequest);
        }
        
        request.ReadBy = _mapper.Map<ParticipantModel>(userRequest.Result);
        var markAsReadRequest = await _chatsService.MarkAsReadAsync(request);
        
        if (!markAsReadRequest.IsSuccess)
        {
            return new HubResponse<MessageModel>(markAsReadRequest);
        }
        
        await SendCounterUpdatedNotification(request.ChatId, request.ReadBy);
        
        return new HubResponse<MessageModel>(markAsReadRequest.Result);
    }
    
    [HubMethodName("LeaveEventChat")]
    public async ValueTask<HubResponse<object>> LeaveEventChat(Guid chatId)
    {
        _logger.LogDebug(
            "LeaveEventChat | ChatId:{ChatId} | ConnectionId:{ConnectionId}",
            GetChatsGroupId(chatId), Context.ConnectionId);
        await _chatsGroupsService.RemoveConnectionFromGroupsAsync(Context.ConnectionId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetChatsGroupId(chatId));

        return new HubResponse<object>(true, 204);
    }

    private async Task SendCounterUpdatedNotification(Guid chatId, ParticipantModel requester)
    {
        throw new NotImplementedException();
        // var chatRequest = await _chatsService.GetChatAsync(chatId, requester);
        // if (!chatRequest.IsSuccess)
        // {
        //     _logger.LogDebug("Error | SendCounterUpdatedNotification | ChatId:{ChatId} | Error:{Error}", chatId,
        //         JsonConvert.SerializeObject(chatRequest));
        //     return;
        // }
        // foreach (var participant in chatRequest.Result.Participants)
        // {
        //     var message = new HubMessage<Dictionary<string, object>>()
        //     {
        //         MessageType = MessageTypes.Notification,
        //
        //         Data = new Dictionary<string, object>()
        //         {
        //             {"eventId", chatRequest.Result.ContextId},
        //             {"unreadMessageCount", chatRequest.Result.MessagesCount - participant.ReadMessagesCount}
        //         },
        //         Event = "ChatCountersUpdated"
        //     };
        //    var notificationTasks = new List<Task>();
        //     var clientsCollection = participant.Role == Roles.Doctor
        //         ? EnvironmentConstants.HisClients
        //         : EnvironmentConstants.PisClients;
        //     foreach (var groupId in clientsCollection.Select(client =>
        //                  $"{client}_{participant.RelativeUserId}"))
        //     {
        //         _logger.LogDebug("SendCounterUpdatedNotification | groupId:{GroupId} | Event:{Event}", groupId,
        //             "ChatCountersUpdated");
        //         notificationTasks.Add(Clients.Group(groupId).ReceiveNotification(message));
        //     }
        //
        //     await Task.WhenAll(notificationTasks);
        // }

    }
    
}