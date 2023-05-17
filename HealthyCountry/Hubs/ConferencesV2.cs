using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.Hubs.Models;
using HealthyCountry.RTC.Models;
using HealthyCountry.Utilities;
using Microsoft.AspNetCore.SignalR;

namespace HealthyCountry.Hubs;

public partial class RtcHub
{
    [HubMethodName("JoinConference")]
    public async Task<HubResponse<object>> JoinConference(BaseConferenceRequest request)
    {
        if (string.IsNullOrEmpty(request?.ConferenceId))
        {
            return new HubResponse<object>(new ServiceResponse<ValidationResult>(new ValidationResult() {Errors = {new ValidationFailure("conferenceId", ValidationMessages.Required)}},
                ServiceResponseStatuses.ValidationFailed));
        }

        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<object>(userRequest);
        }
        
        var message = new HubMessage<CallData>
        {
            Event = "UserJoinedToRoom",
            ConnectionId = Context.ConnectionId,
            Data = new CallData()
            {
                RoomId = request.ConferenceId
            }
        };

        var messageV2 = new HubMessage<Dictionary<string, object>>
        {
            Event = ConferenceEvents.UserJoinedConference.ToString(),
            ConnectionId = Context.ConnectionId,
            MessageType = MessageTypes.Message,
            Data = new Dictionary<string, object>(){
            {
                "ConferenceId", request.ConferenceId
            }}
        };
        
        await Groups.AddToGroupAsync(Context.ConnectionId, request.ConferenceId);

        await Clients.OthersInGroup(request.ConferenceId).SendMessage(message);
        await Clients.OthersInGroup(request.ConferenceId).ReceiveConferenceEvent(messageV2);

        await _conferenceService.AddUserToGroupAsync(request.ConferenceId, _mapper.Map<UserModel>(userRequest.Result));
        return new HubResponse<object>(true);
    }

    [HubMethodName("LeaveConference")]
    public async Task<HubResponse<object>> LeaveConference(BaseConferenceRequest request)
    {
        if (string.IsNullOrEmpty(request?.ConferenceId))
        {
            return new HubResponse<object>()
            {
                IsSuccess = false,
                Code = 400
            };
        }

        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<object>(userRequest);
        }

        var leaveRoomRequest =
            await _conferenceService.LeaveRoomAsync(request.ConferenceId, _mapper.Map<UserModel>(userRequest.Result));

        var tasks = GetLeaveRoomTasks(leaveRoomRequest.Result, userRequest.Result);

        await Task.WhenAll(tasks);
        return new HubResponse<object>(true);
    }

    [HubMethodName("SaveConferenceEvent")]
    public async Task<HubResponse<object>> SaveConferenceEvent(SaveConferenceEventRequest request)
    {
        if (string.IsNullOrEmpty(request?.ConferenceId))
        {
            return new HubResponse<object>()
            {
                IsSuccess = false,
                Code = 400
            };
        }

        var userRequest = await GetUser(false);
        if (!userRequest.IsSuccess)
        {
            return new HubResponse<object>(userRequest);
        }
        
        var room = await _conferenceService.GetOneGroupAsync(request.ConferenceId);

        await _conferenceService.LogEvent(request.EventName ?? "undefined", _mapper.Map<UserModel>(userRequest.Result),
            request.ConferenceId,
            room?.Users?.Count ?? 0);
        return new HubResponse<object>(true);
    }

    [HubMethodName("SendConferenceData")]
    public async Task<HubResponse<object>> SendConferenceData(SendConferenceDataRequest request)
    {
        if (string.IsNullOrEmpty(request?.ConferenceId))
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
                Signal = request.Data,
                RoomId = request.ConferenceId
            }
        };
        var messageV2 = new HubMessage<Dictionary<string, object>>
        {
            Event = ConferenceEvents.UserSentConferenceData.ToString(),
            ConnectionId = Context.ConnectionId,
            MessageType = MessageTypes.Message,
            Data = new Dictionary<string, object>()
            {
                {"ConferenceId", request.ConferenceId},
                {"Data", request.Data}
            }
        };

        await Clients.OthersInGroup(request.ConferenceId).SendMessage(message);
        await Clients.OthersInGroup(request.ConferenceId).ReceiveConferenceEvent(messageV2);
        return new HubResponse<object>(true);
    }
    
    [HubMethodName("SendDummyMessage")]
    public async Task<HubResponse<object>> SendDummyMessage(SendConferenceDataRequest request)
    {
        if (string.IsNullOrEmpty(request?.ConferenceId))
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
                Signal = request.Data,
                RoomId = request.ConferenceId
            }
        };
        var messageV2 = new HubMessage<Dictionary<string, object>>
        {
            Event = ConferenceEvents.UserSentConferenceData.ToString(),
            ConnectionId = Context.ConnectionId,
            MessageType = MessageTypes.Message,
            Data = new Dictionary<string, object>()
            {
                {"ConferenceId", request.ConferenceId},
                {"Data", request.Data}
            }
        };

        await Clients.OthersInGroup(request.ConferenceId).SendMessage(message);
        await Clients.OthersInGroup(request.ConferenceId).ReceiveConferenceEvent(messageV2);
        return new HubResponse<object>(true);
    }
    
    

    // #region mobile calls
    //
    // public async Task MakeUserCall(string roomId)
    // {
    //     var userRequest = await GetUser();
    //
    //     await _conferenceService.InitiateMobileCall(roomId, _mapper.Map<UserModel>(userRequest));
    // }
    //
    // public async Task CancelUserCall(string roomId)
    // {
    //     var userRequest = await GetUser();
    //
    //     await _conferenceService.CancelMobileCall(roomId, _mapper.Map<UserModel>(userRequest),
    //         ReasonCallCancelled.CallerDeclined);
    // }
}