using System.Threading.Tasks;
using EntraIDScanner.API.Controllers.Users.EntraIdUsers;
using EntraIDScanner.API.Models;
using EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces;
using EntraIDScanner.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Graph;
using MongoDB.Driver;
namespace EntraIDScanner.API.Controllers.Devices.EntraIdDevices
{
    [Route("api/devices")]
    [ApiController]
    public class DevicesController : ControllerBase
    {
        private readonly ILogger<DevicesController> _logger;
        private readonly IDeviceRepository _repo;

        public DevicesController(
            IDeviceRepository repo,
            ILogger<DevicesController> logger)
        {
            _repo = repo;
            _logger = logger;
        }
        //Health check for api
        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");
        [HttpGet]
        public async Task<IActionResult> GetDevices()
        {
            try
            {
                _logger.LogInformation("Received request to /api/devices");
                _logger.LogInformation("init creds");
                var devices = await _repo.GetAllDevicesAsync();
                if (devices == null || devices.Count == 0)
                {
                    _logger.LogWarning("No devices found in Microsoft Graph.");
                    return NotFound("No devices found.");
                }

                _logger.LogInformation($"Successfully retrieved {devices.Count} devices.");
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching devices.");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
            ;
     
        }
        [HttpGet("db")]
        public async Task<IActionResult> GetDevicesFromDb()
        {
            try
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var db = client.GetDatabase("EntraDB");
                var collection = db.GetCollection<StoredDevice>("devices");

                var devices = await collection.Find(FilterDefinition<StoredDevice>.Empty).ToListAsync();

                if (devices.Count == 0)
                {
                    _logger.LogWarning("No devices found in MongoDB.");
                    return NotFound("No devices in database.");
                }

                _logger.LogInformation($"Returned {devices.Count} devices from MongoDB.");
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading devices from MongoDB.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }

}

