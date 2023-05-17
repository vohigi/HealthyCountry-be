using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using HealthyCountry.RTC.Models;

namespace HealthyCountry.RTC.Interfaces.Repositories;

public interface ICallGroupsRepository
{
    Task<IEnumerable<GroupModel>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    Task<GroupModel> GetOneAsync(string id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupModel>> GetConnectionGroupsAsync(string connectionId, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupModel>> GetUserGroupsAsync(string userId, CancellationToken cancellationToken = default);
    Task UpdateAsync(GroupModel model, CancellationToken cancellationToken = default);
}