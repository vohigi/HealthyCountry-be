using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities
{
    [Table("websocket-rtc-iceservers"), BsonIgnoreExtraElements]
    public class IceServerEntity : MongoBaseEntity
    {
        public string Urls { get; set; }

        public string Credential { get; set; }

        public string UserName { get; set; }

        public string ServerSecret { get; set; }
        
    }
}
