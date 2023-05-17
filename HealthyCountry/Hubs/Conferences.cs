using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyCountry.Hubs.Models;
using HealthyCountry.RTC.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace HealthyCountry.Hubs;

public partial class RtcHub
{
    [HubMethodName("UserJoinedToRoom")]
    public async Task<HubResponse<object>> UserJoinedToRoom(string roomId, string jsonParams = null)
    {
        if (string.IsNullOrEmpty(roomId))
        {
            return new HubResponse<object>()
            {
                IsSuccess = false,
                Code = 400
            };
        }

        var userRequest = await GetUser();
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<object>(userRequest);
        }

        _logger.LogDebug(
            "UserJoinedToRoom | Role:{Role} | RelativeUserId:{UserId} | ConnectionId:{ConnectionId}",
            userRequest.Result.Role, userRequest.Result.RelativeId, userRequest.Result.ConnectionId);
        
        var message = new HubMessage<CallData>
        {
            Event = "UserJoinedToRoom",
            ConnectionId = Context.ConnectionId,
            Data = new CallData()
            {
                RoomId = roomId
            }
        };
        
        var messageV2 = new HubMessage<Dictionary<string, object>>
        {
            Event = ConferenceEvents.UserJoinedConference.ToString(),
            ConnectionId = Context.ConnectionId,
            MessageType = MessageTypes.Message,
            Data = new Dictionary<string, object>(){
            {
                "ConferenceId", roomId
            }}
        };

        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);

        await Clients.OthersInGroup(roomId).SendMessage(message);
        await Clients.OthersInGroup(roomId).ReceiveConferenceEvent(messageV2);

        await _conferenceService.AddUserToGroupAsync(roomId, _mapper.Map<UserModel>(userRequest.Result));
        return new HubResponse<object>(true);
    }

    [HubMethodName("LeaveRoom")]
    public async Task<HubResponse<object>> LeaveRoom(string roomId, string jsonParams = null)
    {
        if (string.IsNullOrEmpty(roomId))
        {
            return new HubResponse<object>()
            {
                IsSuccess = false,
                Code = 400
            };
        }

        var userRequest = await GetUser();
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<object>(userRequest);
        }
        _logger.LogDebug(
            "LeaveRoom | Role:{Role} | RelativeUserId:{UserId} | ConnectionId:{ConnectionId}",
            userRequest.Result.Role, userRequest.Result.RelativeId, userRequest.Result.ConnectionId);
        
        var leaveRoomRequest = await _conferenceService.LeaveRoomAsync(roomId, _mapper.Map<UserModel>(userRequest.Result));

        var tasks = GetLeaveRoomTasks(leaveRoomRequest.Result, userRequest.Result);

        await Task.WhenAll(tasks);
        return new HubResponse<object>(true);
    }

    [HubMethodName("RoomEvent")]
    public async Task<HubResponse<object>> RoomEvent(string roomId, string eventName, string jsonParams = null)
    {
        if (string.IsNullOrEmpty(roomId))
        {
            return new HubResponse<object>()
            {
                IsSuccess = false,
                Code = 400
            };
        }

        var userRequest = await GetUser();
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<object>(userRequest);
        }
        
        var room = await _conferenceService.GetOneGroupAsync(roomId);

        await _conferenceService.LogEvent(eventName ?? "undefined", _mapper.Map<UserModel>(userRequest.Result), roomId,
            room?.Users?.Count ?? 0);
        return new HubResponse<object>(true);
    }

    [HubMethodName("SendSignal")]
    public async Task<HubResponse<object>> SendSignal(string signal, string roomId)
    {
        if (string.IsNullOrEmpty(roomId))
        {
            return new HubResponse<object>()
            {
                IsSuccess = false,
                Code = 400
            };
        }

        var message = new HubMessage<CallData>
        {
            Event = "SendSignal",
            ConnectionId = Context.ConnectionId,
            Data = new CallData()
            {
                Signal = signal,
                RoomId = roomId
            }
        };
        var messageV2 = new HubMessage<Dictionary<string, object>>
        {
            Event = ConferenceEvents.UserSentConferenceData.ToString(),
            ConnectionId = Context.ConnectionId,
            MessageType = MessageTypes.Message,
            Data = new Dictionary<string, object>()
            {
                {"ConferenceId", roomId},
                {"Data", signal}
            }
        };

        await Clients.OthersInGroup(roomId).SendMessage(message);
        await Clients.OthersInGroup(roomId).ReceiveConferenceEvent(messageV2);
        return new HubResponse<object>(true);
    }

    #region mobile calls

    // public async Task<HubResponse<object>> MakeUserCall(CallPatientRequest request)
    // {
    //     var userRequest = await GetUser(false);
    //     if (!userRequest.IsSuccess)
    //     {
    //         return new HubResponse<object>(userRequest);
    //     }
    //     
    //     request.User = _mapper.Map<UserModel>(userRequest.Result);
    //     await _conferenceService.InitiateMobileCall(request);
    //     return new HubResponse<object>(true);
    // }
    //
    // public async Task<HubResponse<object>> CancelUserCall(string roomId)
    // {
    //     var userRequest = await GetUser(false);
    //     if (!userRequest.IsSuccess)
    //     {
    //         return new HubResponse<object>(userRequest);
    //     }
    //
    //     await _conferenceService.CancelMobileCall(new DeclineCallRequest()
    //     {
    //         Reason = ReasonCallCancelled.CallerDeclined,
    //         Requester = _mapper.Map<UserModel>(userRequest.Result),
    //         RoomId = roomId
    //     });
    //     return new HubResponse<object>(true);
    // }

    #endregion
}