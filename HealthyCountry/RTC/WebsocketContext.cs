using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HealthyCountry.RTC.Entities;
using HealthyCountry.RTC.Entities.Chats;
using HealthyCountry.Utilities.DbContext;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace HealthyCountry.RTC;

public class WebsocketContext : MongoDbContext
{
    public IMongoCollection<GroupEntity> NotificationGroups { get; set; }
    public IMongoCollection<GroupEntity> ChatsGroups { get; set; }
    
    public IMongoCollection<ChatEntity> Chats { get; set; }
    
    public IMongoCollection<MessageEntity> Messages { get; set; }
    public IMongoCollection<GroupEntity> CallGroups { get; set; }
    public IMongoCollection<IceServerEntity> IceServers { get; set; }
    public IMongoCollection<WebsocketEventEntity> WebsocketEvents { get; set; }
    
    public WebsocketContext(IOptions<MongoDbContextOptions<WebsocketContext>> options) : base(options)
    { }

    public override Task MigrateAsync()

        => Task.WhenAll(
            new List<Task>
            {
                NotificationGroups.Indexes.CreateManyAsync(
                    new[]
                    {
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending(x => x.LastUpdatedAt),
                            new CreateIndexOptions {Background = true, ExpireAfter = new TimeSpan(0, 8, 0, 0)}
                        ),
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending("uc.cid"),
                            new CreateIndexOptions {Background = true}
                        ),
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending("uc.ruid"),
                            new CreateIndexOptions {Background = true}
                        )
                    }),
                ChatsGroups.Indexes.CreateManyAsync(
                    new[]
                    {
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending(x => x.LastUpdatedAt),
                            new CreateIndexOptions {Background = true, ExpireAfter = new TimeSpan(0, 8, 0, 0)}
                        ),
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending("uc.cid"),
                            new CreateIndexOptions {Background = true}
                        ),
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending("uc.ruid"),
                            new CreateIndexOptions {Background = true}
                        )
                    }),
                CallGroups.Indexes.CreateManyAsync(
                    new[]
                    {
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending(x => x.LastUpdatedAt),
                            new CreateIndexOptions {Background = true, ExpireAfter = new TimeSpan(0, 2, 0, 0)}
                        ),
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending("uc.cid"),
                            new CreateIndexOptions {Background = true}
                        ),
                        new CreateIndexModel<GroupEntity>
                        (
                            Builders<GroupEntity>.IndexKeys.Ascending("uc.ruid"),
                            new CreateIndexOptions {Background = true}
                        )
                    }),
            });
}