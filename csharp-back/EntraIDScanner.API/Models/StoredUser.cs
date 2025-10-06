using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EntraIDScanner.API.Models
{
    public class StoredUser
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("azureId")]
        public string AzureId { get; set; }

        // What’s the user’s name? Maybe this:
        [BsonElement("displayName")]
        public string DisplayName { get; set; }

        // What field would give you the email?
        [BsonElement("email")]
        public string Email { get; set; }
        [BsonElement("userPrincipalName")]
        public string userPrincipalName { get; set; }
        [BsonElement("jobTitle")]
        public string JobTitle { get; set; }
        [BsonElement("Department")]
        public string Department { get; set; }
        [BsonElement("phone")]
        public string MobilePhone { get; set; }
        [BsonElement("enabled")]
        public bool AccountEnabled { get; set; }
        [BsonElement("created_at")]
        public DateTimeOffset? CreatedDateTime { get; set; }

    }

}
