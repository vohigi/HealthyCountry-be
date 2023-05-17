using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using HealthyCountry.Hubs.Models;
using HealthyCountry.Models;
using HealthyCountry.RTC.Interfaces.Services;
using HealthyCountry.RTC.Models;
using HealthyCountry.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace HealthyCountry.Hubs;

[Authorize]
public partial class RtcHub : Hub<IRtcHub>
{
    private readonly ILogger<RtcHub> _logger;
    private readonly INotificationGroupsService _notificationGroupsService;
    private readonly IConferenceService _conferenceService;
    private readonly IChatsGroupsService _chatsGroupsService;
    private readonly IChatsService _chatsService;
    private readonly IMapper _mapper;

    public RtcHub(
        ILogger<RtcHub> logger,
        INotificationGroupsService notificationGroupsService, 
        IConferenceService conferenceService, 
        IChatsGroupsService chatsGroupsService, 
        IChatsService chatsService,
        IMapper mapper)
    {
        _logger = logger;
        _notificationGroupsService = notificationGroupsService;
        _conferenceService = conferenceService;
        _chatsGroupsService = chatsGroupsService;
        _chatsService = chatsService;
        _mapper = mapper;
    }

    public override async Task OnConnectedAsync()
    {
        // var userRequest = await GetUser();
        // if (!userRequest.IsSuccess)
        // {
        //     throw new HubException("Error getting current resource");
        // }
        //
        // _logger.LogDebug("Connecting clientId:{ClientId} | userId:{UserId}", userRequest.Result.ClientId,
        //     userRequest.Result.RelativeId);
        //
        // var notificationsGroupId = GetNotificationsGroupId(userRequest.Result.ClientId, userRequest.Result.RelativeId);
        // await Groups.AddToGroupAsync(Context.ConnectionId, notificationsGroupId);
        // await _notificationGroupsService.AddUserToGroup(notificationsGroupId, _mapper.Map<UserModel>(userRequest.Result));
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // var notificationConnectionGroups =
        //     await _notificationGroupsService.GetConnectionGroupIdsAsync(Context.ConnectionId);
        // await Task.WhenAll(notificationConnectionGroups.Select(x => Groups.RemoveFromGroupAsync(Context.ConnectionId, x)));
        // await _notificationGroupsService.RemoveConnectionFromGroupsAsync(Context.ConnectionId);
        //
        // var chatConnectionGroups =
        //     await _chatsGroupsService.GetConnectionGroupIdsAsync(Context.ConnectionId);
        // await Task.WhenAll(chatConnectionGroups.Select(x => Groups.RemoveFromGroupAsync(Context.ConnectionId, x)));
        // await _chatsGroupsService.RemoveConnectionFromGroupsAsync(Context.ConnectionId);
        //
        var userRequest = await GetUser(false);
        // if (!userRequest.IsSuccess)
        // {
        //     throw new HubException("Error getting current resource");
        // }
        //
        var tasks = new List<Task>();
        //
        var conferenceGroupsRequest =
            await _conferenceService.OnUserDisconnectAsync(_mapper.Map<UserModel>(userRequest.Result));
        // _logger.LogDebug(
        //     "OnDisconnectedAsync | ClientId:{ClientId} | RelativeUserId:{UserId} | ConnectionId:{ConnectionId}",
        //     userRequest.Result.ClientId, userRequest.Result.RelativeId, userRequest.Result.ConnectionId);
        
        if (conferenceGroupsRequest.IsSuccess && conferenceGroupsRequest.Result.Any())
        {
            foreach (var item in conferenceGroupsRequest.Result)
            {
                tasks.AddRange(GetLeaveRoomTasks(item, userRequest.Result));
            }
        }

        await Task.WhenAll(tasks);

        await base.OnDisconnectedAsync(exception);
    }

    private async Task<ServiceResponse<WebSocketUserModel, ValidationResult>> GetUser(bool throwException = true)
    {
        var user = new WebSocketUserModel
        {
            Role = Enum.Parse<UserRoles>(Context.User?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(x => x.Value).FirstOrDefault() ?? string.Empty),
            ConnectionId = Context.ConnectionId,
            RelativeId = Context.User?.Claims.Where(x => x.Type == ClaimTypes.Name).Select(x => x.Value).FirstOrDefault()
        };
        return new ServiceResponse<WebSocketUserModel, ValidationResult>(user);
    }

    // private static string GetNotificationsGroupId(string clientId, string userId)
    //     => EnvironmentConstants.PisClients.Contains(clientId) ? $"pisclient_{userId}" : $"hisclient_{userId}";
    private static string GetChatsGroupId(Guid chatId)
        => $"chat_{chatId.ToString()}";

    private IEnumerable<Task> GetLeaveRoomTasks(GroupModel item, WebSocketUserModel user)
    {
        var tasks = new List<Task>();

        if (item == null) return tasks;

        var activeUsersCount = item.Users?.Count(x => x.Id != user.RelativeId) ?? 0;
        var message = new HubMessage<CallData>
        {
            Event = "UserDisconnect",
            Data =
            {
                RoomId = item.Id,
                ActiveUsersCount = activeUsersCount
            },
        };

        var messageV2 = new HubMessage<Dictionary<string, object>>
        {
            Event = ConferenceEvents.UserLeftConference.ToString(),
            MessageType = MessageTypes.Message,
            Data = new Dictionary<string, object>
            {
                {
                    "ConferenceId", item.Id
                },
                {
                    "ActiveUsersCount", activeUsersCount
                }
            }
        };
        
        var newTask = Task.Run(async () =>
        {
            await Clients.OthersInGroup(item.Id).ReceiveConferenceEvent(messageV2);
        });
        var oldTask = Task.Run(async () =>
        {
            await Clients.OthersInGroup(item.Id).SendMessage(message);
        });

        tasks.Add(Groups.RemoveFromGroupAsync(Context.ConnectionId, item.Id));

        tasks.Add(newTask);
        tasks.Add(oldTask);

        return tasks;
    }
}