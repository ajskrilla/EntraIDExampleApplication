using EntraIDScanner.API.Models;
using MongoDB.Driver;

namespace EntraIDScanner.API.Data.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _db;
        public MongoDbContext(IConfiguration config)
        {
            var connString = config.GetConnectionString("MongoDb");
            var dbName = config.GetValue<string>("MongoDbDatabaseName");
            var client = new MongoClient(connString);
            _db = client.GetDatabase(dbName);
        }
        // These are the 3 collections so far for the context
        public IMongoCollection<StoredUser> Users => _db.GetCollection<StoredUser>("users");
        public IMongoCollection<StoredDevice> Devices => _db.GetCollection<StoredDevice>("devices");
        public IMongoCollection<SyncStatus> SyncStatus => _db.GetCollection<SyncStatus>("syncStatus");
    }
}
