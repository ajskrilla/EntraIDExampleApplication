using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EntraIDScanner.API.Services.EntraId;

namespace EntraIDScanner.API.Controllers.Util
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly ILogger<SyncController> _logger;
        private readonly SyncService _syncService;

        // Pull in SyncService (and let its own ILogger<MongoDbContext>, IUserRepository, 
        // IDeviceRepository, etc. be injected by DI)
        public SyncController(
            ILogger<SyncController> logger,
            SyncService syncService)
        {
            _logger = logger;
            _syncService = syncService;
        }

        [HttpPost]
        public async Task<IActionResult> Sync()
        {
            _logger.LogInformation("Sync endpoint called.");
            await _syncService.SyncAsync();
            return Ok(new { message = "Entra ID data synced successfully." });
        }
    }
}
