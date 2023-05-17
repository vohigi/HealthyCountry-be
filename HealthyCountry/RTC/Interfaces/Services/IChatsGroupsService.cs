using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HealthyCountry.RTC.Models;

namespace HealthyCountry.RTC.Interfaces.Services;

public interface IChatsGroupsService
{
    Task<GroupModel> AddUserToGroup(string groupId, UserModel user,
        CancellationToken cancellationToken = default);

    Task<IEnumerable<string>> GetConnectionGroupIdsAsync(string connectionId,
        CancellationToken cancellationToken = default);

    Task RemoveConnectionFromGroupsAsync(string connectionId,
        CancellationToken cancellationToken = default);

}