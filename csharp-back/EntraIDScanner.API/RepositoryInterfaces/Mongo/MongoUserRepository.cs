namespace EntraIDScanner.API.RepositoryInterfaces.Mongo
{
    using EntraIDScanner.API.Models;
    using EntraIDScanner.API.RepositoryInterfaces.StoredDatabaseRepository;
    using Microsoft.Extensions.Logging;
    using Microsoft.Kiota.Abstractions.Extensions;
    using MongoDB.Driver;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class MongoUserRepository : IStoredDatabaseUserRepository
    {
        private readonly IMongoCollection<StoredUser> _col;
        private readonly ILogger<MongoUserRepository> _logger;

        public MongoUserRepository(
            ILogger<MongoUserRepository> logger,
            IMongoClient mongoClient) 
        {
            _logger = logger;
            var db = mongoClient.GetDatabase("EntraDB");
            _col = db.GetCollection<StoredUser>("users");
        }

        public async Task<List<StoredUser>> GetAllAsync()
        {
            _logger.LogInformation("Reading all users from MongoDB");
            return await _col.Find(FilterDefinition<StoredUser>.Empty).ToListAsync();
        }

        public async Task<StoredUser> GetByIdAsync(string azureId)
        {
            _logger.LogInformation("Reading user {Id} from MongoDB", azureId);
            var filter = Builders<StoredUser>.Filter.Eq(u => u.AzureId, azureId);
            return await _col.Find(filter).FirstOrDefaultAsync();
        }

        public async Task SaveManyAsync(IEnumerable<StoredUser> users)
        {
            _logger.LogInformation("Saving {Count} users to MongoDB", users.AsList().Count);
            await _col.DeleteManyAsync(FilterDefinition<StoredUser>.Empty);  // optional clear
            await _col.InsertManyAsync(users);
        }

        public async Task DeleteAllAsync()
        {
            _logger.LogInformation("Dropping all users in MongoDB");
            await _col.DeleteManyAsync(FilterDefinition<StoredUser>.Empty);
        }

        public async Task<StoredUser> GetSingleUser(string identifier)
        {
            _logger.LogInformation($"looking for a single user with id: {identifier}");
            var byIdFilter = Builders<StoredUser>.Filter.Eq(u => u.AzureId, identifier);
            var byUpnFilter = Builders<StoredUser>.Filter.Eq(u => u.userPrincipalName, identifier);
            var combined = Builders<StoredUser>.Filter.Or(byIdFilter, byUpnFilter);
            return await _col
                         .Find(combined)
                         .FirstOrDefaultAsync();
        }
    }
}
