using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HealthyCountry.RTC.Entities
{
    public class MongoBaseEntity
    {
        /// <summary>
        /// System document Id
        /// </summary> 
        [BsonId, BsonRepresentation(BsonType.String)]
        public virtual Guid Id { get; set; } = Guid.NewGuid();

    }
}
