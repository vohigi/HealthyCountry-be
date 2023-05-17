using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation.Results;
using HealthyCountry.RTC.Models;
using HealthyCountry.Utilities;

namespace HealthyCountry.RTC.Interfaces.Services
{
    public interface IConferenceService
    {
        Task<RoomsInfoResponseModel> GetGroupsInfoAsync(RoomsInfoRequestModel request);

        Task<GroupModel> AddUserToGroupAsync(string groupId, UserModel user);

        Task<IEnumerable<string>> GetConnectionGroupIdsAsync(string connectionId,
            CancellationToken cancellationToken = default);

        Task RemoveConnectionFromGroupsAsync(string connectionId,
            CancellationToken cancellationToken = default);
        
        Task<ServiceResponse<GroupModel, ValidationResult>> LeaveRoomAsync(string groupId, UserModel user);

        Task<ServiceResponse<IEnumerable<GroupModel>, ValidationResult>> OnUserDisconnectAsync(UserModel user);

        Task<GroupModel> GetOneGroupAsync(string groupId);

        Task<ServiceResponse<WebRtcConfigModel, ValidationResult>> GetWebRtcConfigAsync(string userId);

        Task LogEvent(string eventName, UserModel user, string roomId, int activeUsersCount);
        
        #region mobile calls
        
        // Task InitiateMobileCall(CallPatientRequest request);
        // Task CancelMobileCall(DeclineCallRequest request);
        // Task<bool> GetCallStatus(string roomId);
        
        #endregion
        
    }

}
