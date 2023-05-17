namespace HealthyCountry.RTC.Repositories;

// public class ChatNotificationTaskRepository : QueueRepository<ChatNotificationTask>, IChatNotificationQueueRepository
// {
//     public ChatNotificationTaskRepository(QueueContext context) : base(context.ChatNotificationTasks)
//     {
//     }
//
//
//     public async Task<List<ChatNotificationTask>> BlockAndGetAllChatUserTasksAsync(ChatNotificationTask originTask,
//         CancellationToken cancellationToken = default)
//     {
//         var affectedTasks = await Collection.Find(x =>
//                 x.RelativeUserId == originTask.RelativeUserId && x.ChatId == originTask.ChatId &&
//                 x.CreatedAt >= originTask.CreatedAt)
//             .ToListAsync(cancellationToken: cancellationToken);
//         await Collection.UpdateManyAsync(
//             Builders<ChatNotificationTask>.Filter.In(x => x.Id, affectedTasks.Select(s => s.Id)),
//             Builders<ChatNotificationTask>.Update.Set(d => d.DelayedTill,
//                 originTask.UserRole == (int)Roles.Doctor ? DateTime.Now.AddMinutes(5) : DateTime.Now.AddSeconds(20)),
//             cancellationToken: cancellationToken);
//         return affectedTasks;
//     }
//
//     public async Task CompleteManyAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default)
//     {
//         await Collection.DeleteManyAsync(x => ids.Contains(x.Id), cancellationToken: cancellationToken);
//     }
//
//     public async Task CompleteForMessageAsync(Guid messageId, string relativeUserId,
//         CancellationToken stoppingToken = default)
//     {
//         await Collection.DeleteOneAsync(x => x.MessageId == messageId && x.RelativeUserId == relativeUserId,
//             cancellationToken: stoppingToken);
//     }
// }