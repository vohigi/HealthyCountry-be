using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace HealthyCountry.RTC.Entities.Queues;

// [BsonIgnoreExtraElements, Table("rtc_chat_notification_tasks")]
// public class ChatNotificationTask : QueueElementEntity
// {
//     [BsonRepresentation(BsonType.String), BsonElement("mid")]
//     public Guid MessageId { get; set; }
//
//     [BsonRepresentation(BsonType.String), BsonElement("chid")]
//     public Guid ChatId { get; set; }
//
//     [BsonRepresentation(BsonType.String), BsonElement("cid")]
//     public string ContextId { get; set; }
//
//     [BsonRepresentation(BsonType.String), BsonElement("rlid")]
//     public string RelativeUserId { get; set; }
//
//     [BsonElement("ur")] public byte UserRole { get; set; }
//
//
//     public ChatNotificationTask()
//     {
//     }
//
//     public ChatNotificationTask(Guid messageId, Guid chatId, string relativeUserId, byte userRole, DateTime delayedTill,
//         string contextId)
//     {
//         MessageId = messageId;
//         RelativeUserId = relativeUserId;
//         UserRole = userRole;
//         DelayedTill = delayedTill;
//         ChatId = chatId;
//         ContextId = contextId;
//     }
// }
//
// public class ChatNotificationTaskConfiguration : QueueElementEntityConfiguration<ChatNotificationTask>
// {
//     public ChatNotificationTaskConfiguration(IMongoCollection<ChatNotificationTask> collection) : base(collection)
//     {
//     }
//
//     protected override IEnumerable<CreateIndexModel<ChatNotificationTask>> IndicesConfiguration()
//     {
//         if (base.IndicesConfiguration() is not List<CreateIndexModel<ChatNotificationTask>> indices) return null;
//
//         indices.Add(new CreateIndexModel<ChatNotificationTask>
//         (
//             Builders<ChatNotificationTask>.IndexKeys.Combine(
//                 Builders<ChatNotificationTask>.IndexKeys.Ascending(x => x.CreatedAt),
//                 Builders<ChatNotificationTask>.IndexKeys.Ascending(x => x.ChatId),
//                 Builders<ChatNotificationTask>.IndexKeys.Ascending(x => x.RelativeUserId)
//             ),
//             new CreateIndexOptions { Background = true }));
//         return indices;
//     }
// }