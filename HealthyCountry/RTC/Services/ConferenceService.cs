using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Interfaces.Services;
using HealthyCountry.RTC.Models;
using HealthyCountry.Utilities;

namespace HealthyCountry.RTC.Services
{
    //TODO convert all return types to serviceResponse
    public class ConferenceService : IConferenceService
    {
        private readonly ICallGroupsRepository _callGroupsRepository;
        private readonly IEventsRepository _eventsRepository;
        private readonly IIceServerRepository _iceServerRepository;
        // private readonly IEventActionsServiceProvider
        //     _eventActionsServiceProvider;
        //private readonly Lazy<ConnectionMultiplexer> _connectionMultiplexer;
        private const string RtcCallStatus = "rtc:call-status-";
        //private readonly CallSettings _callSettings;

        public ConferenceService(IEventsRepository eventsRepository,
            IIceServerRepository iceServerRepository,
            ICallGroupsRepository callGroupsRepository)
        {
            _eventsRepository = eventsRepository;
            _iceServerRepository = iceServerRepository;
            _callGroupsRepository = callGroupsRepository;
            //_connectionMultiplexer = connectionMultiplexer;
            //_callSettings = callOptions.Value;
        }

        public async Task<RoomsInfoResponseModel> GetGroupsInfoAsync(RoomsInfoRequestModel request)
        {
            var results = await _callGroupsRepository.GetAsync(request.RoomIds);

            var response = new RoomsInfoResponseModel(results.ToList());

            return response;
        }

        public async Task<GroupModel> AddUserToGroupAsync(string groupId, UserModel user)
        {
            var group = await _callGroupsRepository.GetOneAsync(groupId) ?? new GroupModel()
            {
                Id = groupId
            };

            group.Users ??= new List<UserModel>();

            group.Users.Add(user);

            await _callGroupsRepository.UpdateAsync(group);

            // await _eventActionsServiceProvider.ProcessActionAsync(new ProcessActionRequest()
            // {
            //     Action = ProcessActionRequest.Actions.UserEnteredOnlineRoom,
            //     ClientId = user.ClientId,
            //     UserId = user.Id,
            //     EventId = groupId,
            //     ActiveUsersCount = group.Users?.Select(x => x.Id).Distinct().Count() ?? 0,
            //     ActiveUsersExceptPerformerCount =
            //         group.Users?.Select(x => x.Id).Distinct().Count(x => x != user.Id) ?? 0
            // });

            _ = Task.Run(async () => { await LogEvent("UserJoinedToRoom", user, groupId, group?.Users?.Count ?? 0); });

            return group;
        }

        public async Task<IEnumerable<string>> GetConnectionGroupIdsAsync(string connectionId,
            CancellationToken cancellationToken = default)
        {
            var groups = await _callGroupsRepository.GetConnectionGroupsAsync(connectionId, cancellationToken);
            return groups.Select(x => x.Id);
        }

        public async Task RemoveConnectionFromGroupsAsync(string connectionId,
            CancellationToken cancellationToken = default)
        {
            var groups = await _callGroupsRepository.GetConnectionGroupsAsync(connectionId, cancellationToken);
            foreach (var wsGroup in groups)
            {
                wsGroup.Users.RemoveAll(x => x.ConnectionId == connectionId);
                await _callGroupsRepository.UpdateAsync(wsGroup, cancellationToken);
            }
        }

        public async Task<ServiceResponse<IEnumerable<GroupModel>, ValidationResult>> OnUserDisconnectAsync(UserModel user)
        {
            var connectionGroups = await _callGroupsRepository.GetConnectionGroupsAsync(user.ConnectionId);

            var groups = connectionGroups.ToList();

            foreach (var group in groups)
            {
                group.Users?.RemoveAll(x => x.Id == user.Id && x.ConnectionId == user.ConnectionId);
                await _callGroupsRepository.UpdateAsync(group);

                // await _eventActionsServiceProvider.ProcessActionAsync(new ProcessActionRequest()
                // {
                //     Action = ProcessActionRequest.Actions.UserLeftOnlineRoom,
                //     ClientId = user.ClientId,
                //     UserId = user.Id,
                //     EventId = group.Id,
                //     ActiveUsersCount = group.Users?.Select(x => x.Id).Distinct().Count() ?? 0,
                //     ActiveUsersExceptPerformerCount =
                //         group.Users?.Select(x => x.Id).Distinct().Count(x => x != user.Id) ?? 0
                // });
                _ = Task.Run(async () =>
                {
                    await LogEvent("UserDisconnected", user, group.Id, group?.Users?.Count ?? 0);
                });
            }

            return new ServiceResponse<IEnumerable<GroupModel>, ValidationResult>(groups);
        }

        public async Task<GroupModel> GetOneGroupAsync(string groupId) =>
            await _callGroupsRepository.GetOneAsync(groupId);

        public async Task<ServiceResponse<GroupModel, ValidationResult>> LeaveRoomAsync(string groupId, UserModel user)
        {
            var group = await _callGroupsRepository.GetOneAsync(groupId);

            if (group is {Users: { }})
            {
                if (user.Id != null)
                {
                    group.Users.RemoveAll(x => x.Id == user.Id);
                }

                if (user.ConnectionId != null)
                {
                    group.Users.RemoveAll(x => x.ConnectionId == user.ConnectionId);
                }
                await _callGroupsRepository.UpdateAsync(group);
                // await _eventActionsServiceProvider.ProcessActionAsync(new ProcessActionRequest()
                // {
                //     Action = ProcessActionRequest.Actions.UserLeftOnlineRoom,
                //     ClientId = user.ClientId,
                //     UserId = user.Id,
                //     EventId = group.Id,
                //     ActiveUsersCount = group.Users?.Select(x => x.Id).Distinct().Count() ?? 0,
                //     ActiveUsersExceptPerformerCount =
                //         group.Users?.Select(x => x.Id).Distinct().Count(x => x != user.Id) ?? 0
                // });
                
                return new ServiceResponse<GroupModel, ValidationResult>(group);
            }

            _ = Task.Run(async () => { await LogEvent("LeaveRoom", user, groupId, group?.Users?.Count ?? 0); });
            return new ServiceResponse<GroupModel, ValidationResult>(
                new ValidationResult() {Errors = {new ValidationFailure("group", "Entity not found")}},
                ServiceResponseStatuses.NotFound);
        }

        /// <summary>
        /// Непонятное логирование в монгу, было в предидущей версии сервиса,
        /// возможно нужно убрать
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="user"></param>
        /// <param name="roomId"></param>
        /// <param name="activeUsersCount"></param>
        public async Task LogEvent(string eventName, UserModel user, string roomId, int activeUsersCount)
        {
            await _eventsRepository.CreateEvent(new WebSocketEventModel
            {
                ActiveUserCount = activeUsersCount,
                Date = DateTime.UtcNow,
                EventName = eventName,
                RoomId = roomId,
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Role = user.Role
            });
        }

        public async Task<ServiceResponse<WebRtcConfigModel, ValidationResult>> GetWebRtcConfigAsync(string userId)
        {
            var iceServers = await _iceServerRepository.GetServers(userId);

            var config = new WebRtcConfigModel
            {
                IceServers = iceServers,
                IceTransportPolicy = "relay"
            };

            return new ServiceResponse<WebRtcConfigModel, ValidationResult>(config);
        }

        #region calls

        // public async Task InitiateMobileCall(CallPatientRequest request)
        // {
        //     var redisKey = GetRoomCallKey(request.RoomId);
        //
        //     //try to send notification
        //     var serviceResponse = await _eventActionsServiceProvider.ProcessActionAsync(new ProcessActionRequest
        //     {
        //         Action = ProcessActionRequest.Actions.ResourceInitiatedCall,
        //         ClientId = request.User.ClientId,
        //         UserId = request.User.Id,
        //         EventId = request.RoomId,
        //         CallRecipientId = request.PatientId
        //     });
        //     
        //     if (serviceResponse.IsSuccess)
        //     {
        //         //save redis key as call info
        //         await _connectionMultiplexer.Value.GetDatabase(4).StringSetAsync(redisKey, request.PatientId ?? "",
        //             TimeSpan.FromSeconds(_callSettings.CallingTimeout));
        //     }
        //     //do nothing
        // }
        //
        // public async Task CancelMobileCall(DeclineCallRequest request)
        // {
        //     var redisKey = GetRoomCallKey(request.RoomId);
        //     //get redis key. If not null - stop
        //     var isCalled = await _connectionMultiplexer.Value.GetDatabase(4).KeyExistsAsync(redisKey);
        //     if(isCalled) return;
        //     var patientId = await _connectionMultiplexer.Value.GetDatabase(4).StringGetAsync(redisKey);
        //     var serviceResponse = await _eventActionsServiceProvider.ProcessActionAsync(new 
        //         ProcessActionRequest()
        //         {
        //             Action = request.Reason switch
        //             {
        //                 ReasonCallCancelled.CallerDeclined => ProcessActionRequest.Actions.ResourceCancelledCall,
        //                 ReasonCallCancelled.ReceiverDeclined => ProcessActionRequest.Actions.PatientDeclinedCall,
        //                 _ => throw new ArgumentOutOfRangeException(nameof(request.Reason), request.Reason, null)
        //             },
        //             ClientId = request.Requester.ClientId,
        //             UserId = request.Requester.Id,
        //             EventId = request.RoomId,
        //             CallRecipientId = patientId.ToString()
        //         });
        //     if (serviceResponse.IsSuccess)
        //     {
        //         //remove redis call info key
        //         await _connectionMultiplexer.Value.GetDatabase(4).KeyDeleteAsync(redisKey);
        //     }
        //     //do nothing
        // }
        //
        // public async Task<bool> GetCallStatus(string roomId)
        // {
        //     var redisKey = GetRoomCallKey(roomId);
        //
        //     return await _connectionMultiplexer.Value.GetDatabase(4).KeyExistsAsync(redisKey);
        // }

        private static string GetRoomCallKey(string roomId)
        {
            var userCacheKey = $"{RtcCallStatus}_{roomId}";

            return userCacheKey.ToLower();
        }
        #endregion
        
    }
}