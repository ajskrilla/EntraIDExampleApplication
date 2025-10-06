using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EntraIDScanner.API.Models
{
    public class SyncStatus
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("lastSyncedAt")]
        public DateTime LastSyncedAt { get; set; }
    }
}
