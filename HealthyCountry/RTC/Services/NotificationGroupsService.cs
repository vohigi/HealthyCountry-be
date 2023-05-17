using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Interfaces.Services;
using HealthyCountry.RTC.Models;

namespace HealthyCountry.RTC.Services;

public class NotificationGroupsService : INotificationGroupsService
{
    private readonly INotificationGroupsRepository _groupsRepository;

    public NotificationGroupsService(INotificationGroupsRepository groupsRepository, IMapper mapper)
    {
        _groupsRepository = groupsRepository;
    }

    public async Task<GroupModel> AddUserToGroup(string groupId, UserModel user,
        CancellationToken cancellationToken = default)
    {
        var group = await _groupsRepository.GetOneAsync(groupId, cancellationToken) ?? new GroupModel()
        {
            Id = groupId,
            Users = new List<UserModel>()
        };
        
        if (group.Users.FirstOrDefault(x =>
                x.ConnectionId == user.ConnectionId && x.Role == user.Role && x.Id == user.Id) is null)
        {
            group.Users.Add(user);
        }

        await _groupsRepository.UpdateAsync(group, cancellationToken);

        return group;
    }

    public async Task<IEnumerable<string>> GetConnectionGroupIdsAsync(string connectionId,
        CancellationToken cancellationToken = default)
    {
        var groups = await _groupsRepository.GetConnectionGroupsAsync(connectionId, cancellationToken);
        return groups.Select(x => x.Id);
    }

    public async Task RemoveConnectionFromGroupsAsync(string connectionId,
        CancellationToken cancellationToken = default)
    {
        var groups = await _groupsRepository.GetConnectionGroupsAsync(connectionId, cancellationToken);
        foreach (var wsGroup in groups)
        {
            wsGroup.Users.RemoveAll(x => x.ConnectionId == connectionId);
            await _groupsRepository.UpdateAsync(wsGroup, cancellationToken);
        }
    }
}