using MongoDB.Driver;


namespace EntraIDScanner.API.Services.EntraId
{

    public class CredentialService
    {
        private readonly IMongoCollection<Credential> _collection;
        private readonly ILogger<CredentialService> _logger;

        public CredentialService(ILogger<CredentialService> logger)
        {
            _logger = logger;

            _logger.LogInformation(" Connecting to MongoDB...");
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("EntraDB");

            _collection = db.GetCollection<Credential>("credentials");
            _logger.LogInformation("MongoDB connection established.");
        }

        public Credential GetLatestCredential()
        {
            _logger.LogInformation(" Fetching latest credentials...");

            var credential = _collection
                .Find(Builders<Credential>.Filter.Empty)
                .SortByDescending(c => c.CreatedAt)
                .FirstOrDefault();

            if (credential == null)
            {
                _logger.LogWarning(" No credentials found in MongoDB.");
            }
            else
            {
                _logger.LogInformation($" Credential loaded. Client ID: {credential.ClientId}");
            }

            return credential;
        }
    }

}