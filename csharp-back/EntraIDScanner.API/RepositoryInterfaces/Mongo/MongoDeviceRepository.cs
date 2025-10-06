namespace EntraIDScanner.API.RepositoryInterfaces.Mongo
{

    using EntraIDScanner.API.Models;
    using EntraIDScanner.API.RepositoryInterfaces.StoredDatabaseRepository;
    using Microsoft.Extensions.Logging;
    using Microsoft.Kiota.Abstractions.Extensions;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MongoDeviceRepository : IStoredDatabaseDeviceRepository
    {
        private readonly IMongoCollection<StoredDevice> _col;
        private readonly ILogger<MongoUserRepository> _logger;

        public MongoDeviceRepository(
            ILogger<MongoUserRepository> logger,
            IMongoClient mongoClient)
        {
            _logger = logger;
            var db = mongoClient.GetDatabase("EntraDB");
            _col = db.GetCollection<StoredDevice>("users");
        }

        public async Task<List<StoredDevice>> GetAllAsync()
        {
            _logger.LogInformation("Reading all users from MongoDB");
            return await _col.Find(FilterDefinition<StoredDevice>.Empty).ToListAsync();
        }

        public async Task<StoredDevice> GetByIdAsync(string azureId)
        {
            _logger.LogInformation("Reading user {Id} from MongoDB", azureId);
            var filter = Builders<StoredDevice>.Filter.Eq(d => d.DeviceId, azureId);
            return await _col.Find(filter).FirstOrDefaultAsync();
        }

        public async Task SaveManyAsync(IEnumerable<StoredDevice> devices)
        {
            _logger.LogInformation($"Saving {devices.AsList().Count} users to MongoDB");
            await _col.DeleteManyAsync(FilterDefinition<StoredDevice>.Empty);  // optional clear
            await _col.InsertManyAsync(devices);
        }

        public async Task DeleteAllAsync()
        {
            _logger.LogInformation("Dropping all users in MongoDB");
            await _col.DeleteManyAsync(FilterDefinition<StoredDevice>.Empty);
        }
    }
}
