using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace EntraIDScanner.API.Controllers.Administration
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly ILogger<AdminController> _logger;

        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }
        // Fix this so that it looks at MongoDB delete DB
        // Have there be arguments on dropping the collections in Mongo DB
        // Also connection string 
        [HttpDelete("reset")]
        public IActionResult ResetDatabase()
        {
            try
            {
                var client = new MongoClient("mongodb://localhost:27017");
                var db = client.GetDatabase("EntraDB");

                db.DropCollection("users");
                db.DropCollection("devices");
                db.DropCollection("syncStatus");
                db.DropCollection("credentials");

                _logger.LogInformation("Database reset: All collections dropped.");
                return Ok(new { message = "Database successfully reset." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error resetting database.");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
