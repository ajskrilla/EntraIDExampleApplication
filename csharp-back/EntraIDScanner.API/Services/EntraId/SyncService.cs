using EntraIDScanner.API.Controllers.Devices.EntraIdDevices;
using EntraIDScanner.API.Data.MongoDB;
using EntraIDScanner.API.Models;
using EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;
using System.Threading.Tasks;

namespace EntraIDScanner.API.Services.EntraId
{
    public class SyncService
    {
        private readonly ILogger<SyncService> _logger;
        private readonly MongoDbContext _context;
        private readonly IUserRepository _userRepo;
        private readonly IDeviceRepository _deviceRepo;


        public SyncService(
            ILogger<SyncService> logger,
            MongoDbContext context,
            IUserRepository userRepo,
            IDeviceRepository deviceRepo)
        {
            _logger = logger;
            _context = context;
            _userRepo = userRepo;
            _deviceRepo = deviceRepo;
        }

        public async Task SyncAsync()
        {
            _logger.LogInformation(" Starting Entra ID sync...");
            var users = await _userRepo.GetAllUsersAsync();
            var devices = await _deviceRepo.GetAllDevicesAsync();

            // Sync timestamp
            _context.SyncStatus.DeleteMany(FilterDefinition<SyncStatus>.Empty);
            _context.SyncStatus.InsertOne(new SyncStatus { LastSyncedAt = DateTime.UtcNow });
            _logger.LogInformation(" Last sync timestamp updated.");

            if (users?.Count > 0)
            {
                _context.Users.DeleteMany(Builders<StoredUser>.Filter.Empty);
                var mappedUsers = users.Select(u => new StoredUser
                {
                    Id = ObjectId.GenerateNewId(),
                    AzureId = u.Id,
                    DisplayName = u.DisplayName,
                    Email = u.Mail,
                    userPrincipalName = u.UserPrincipalName,
                    JobTitle = u.JobTitle,
                    Department = u.Department,
                    MobilePhone = u.MobilePhone,
                    AccountEnabled = u.AccountEnabled.GetValueOrDefault(),
                    CreatedDateTime = u.CreatedDateTime
                }).ToList();
                _context.Users.InsertMany(mappedUsers); 
                _logger.LogInformation($" Synced {users.Count} users to MongoDB.");
            }

            if (devices?.Count > 0)
            {
                _context.Devices.DeleteMany(Builders<StoredDevice>.Filter.Empty);
                
                var mappedDevices = devices.Select(d => new StoredDevice
                {
                    Id = ObjectId.GenerateNewId(),
                    AzureDeviceId = d.Id,
                    DisplayName = d.DisplayName,
                    OperatingSystem = d.OperatingSystem,
                    OperatingSystemVersion = d.OperatingSystemVersion,
                    DeviceId = d.DeviceId,
                    IsCompliant = d.IsCompliant.GetValueOrDefault(),
                    IsManaged = d.IsManaged.GetValueOrDefault(),
                    TrustType = d.TrustType
                }).ToList();
                _context.Devices.InsertMany(mappedDevices);
                _logger.LogInformation($" Synced {devices.Count} devices to MongoDB.");
            }

            _logger.LogInformation(" Entra ID sync complete.");
        }
    }
}
