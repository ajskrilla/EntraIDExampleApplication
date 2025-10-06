using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EntraIDScanner.API.Services
{
    [BsonIgnoreExtraElements] // Instructs driver to ignore fields like __v!!!
    public class Credential
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("tenantId")]
        public string TenantId { get; set; }

        [BsonElement("clientId")]
        public string ClientId { get; set; }

        [BsonElement("clientSecret")]
        public string ClientSecret { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

    }
}