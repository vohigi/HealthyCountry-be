using System;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities
{
    [Table("websocket-rtc-events"), BsonIgnoreExtraElements]
    public class WebsocketEventEntity : MongoBaseEntity
    {
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime Date { get; set; }

        public string EventName { get; set; }

        public string RoomId { get; set; }

        public string UserId { get; set; }

        public int ActiveUserCount { get; set; }

        public string ResourceId { get; set; }
    }
}
