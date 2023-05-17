using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using HealthyCountry.RTC.Entities;
using HealthyCountry.RTC.Interfaces.Repositories;
using HealthyCountry.RTC.Models;
using MongoDB.Driver;

namespace HealthyCountry.RTC.Repositories;

public class CallGroupsRepository : ICallGroupsRepository
{
    private readonly WebsocketContext _context;
    private readonly IMapper _mapper;

    public CallGroupsRepository(WebsocketContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<GroupModel>> GetAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
    {
        var entities = await _context.CallGroups.Find(x => ids.Contains(x.Id))
            .ToListAsync(cancellationToken: cancellationToken);
        return entities.Select(_mapper.Map<GroupModel>);
    }
    
    public async Task<GroupModel> GetOneAsync(string id, CancellationToken cancellationToken = default)
    {
        var entity = await _context.CallGroups.Find(x => x.Id == id)
            .FirstOrDefaultAsync(cancellationToken: cancellationToken);
        return _mapper.Map<GroupModel>(entity);
    }
    
    public async Task<IEnumerable<GroupModel>> GetConnectionGroupsAsync(string connectionId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.CallGroups
            .Find(x => x.UserConnections.Any(z => z.ConnectionId == connectionId)).ToListAsync(cancellationToken);
        return entities.Select(_mapper.Map<GroupModel>);
    }

    public async Task<IEnumerable<GroupModel>> GetUserGroupsAsync(string userId, CancellationToken cancellationToken = default)
    {
        var entities = await _context.CallGroups
            .Find(x => x.UserConnections.Any(z => z.RelativeUserId == userId)).ToListAsync(cancellationToken);
        return entities.Select(_mapper.Map<GroupModel>);
    }
    
    public async Task UpdateAsync(GroupModel model, CancellationToken cancellationToken = default)
    {
        var entity = _mapper.Map<GroupEntity>(model);
        entity.LastUpdatedAt = DateTime.Now;
        await _context.CallGroups.FindOneAndReplaceAsync<GroupEntity>(x => x.Id == entity.Id, entity,
            new FindOneAndReplaceOptions<GroupEntity>() {IsUpsert = true},
            cancellationToken);
    }
}