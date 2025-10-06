using EntraIDScanner.API.RepositoryInterfaces.StoredDatabaseRepository;
using Microsoft.AspNetCore.Mvc;

namespace EntraIDScanner.API.Controllers.Devices.DatabaseDevices
{
    public class MongoDbDeviceController : Controller
    {
        private readonly IStoredDatabaseDeviceRepository _db;
        private readonly ILogger<MongoDbDeviceController> _logger;
        public MongoDbDeviceController(
                IStoredDatabaseDeviceRepository db,
                ILogger<MongoDbDeviceController> log)
        {
            _db = db;
            _logger = log;
        }
        [HttpGet("allmongodbdevices")]
        public async Task<IActionResult> GetDevicesFromDb()
        {
            try
            {
                var devices = await _db.GetAllAsync();
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
        [HttpGet("singlemongodbdevice")]
        public async Task<IActionResult> GetDevicesFromDb()
        {
            try
            {
                var devices = await _db.GetAllAsync();
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
