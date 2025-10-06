using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace EntraIDScanner.API.Models
{
    public class StoredDevice
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("azureDeviceId")]
        public string AzureDeviceId { get; set; }

        [BsonElement("displayName")]
        public string DisplayName { get; set; }

        [BsonElement("os")]
        public string OperatingSystem { get; set; }
        [BsonElement("os_ver")]
        public string OperatingSystemVersion { get; set; }
        [BsonElement("deviceId")]
        public string DeviceId { get; set; }
        [BsonElement("is_comp")]
        public bool IsCompliant { get; set; }
        [BsonElement("is_man")]
        public bool IsManaged { get; set; }
        [BsonElement("trust_type")]
        public string TrustType { get; set; }

    }

}
