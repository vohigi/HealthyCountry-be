using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HealthyCountry.RTC.Models;

namespace HealthyCountry.RTC.Interfaces.Repositories;

public interface INotificationGroupsRepository
{
    Task<GroupModel> GetOneAsync(string id, CancellationToken cancellationToken);
    Task<IEnumerable<GroupModel>> GetConnectionGroupsAsync(string connectionId, CancellationToken cancellationToken);
    Task<IEnumerable<GroupModel>> GetUserGroupsAsync(string userId, CancellationToken cancellationToken);
    Task UpdateAsync(GroupModel model, CancellationToken cancellationToken);
}