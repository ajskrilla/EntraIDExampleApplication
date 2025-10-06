using EntraIDScanner.API.RepositoryInterfaces;
using EntraIDScanner.API.Services;
using Microsoft.AspNetCore.Mvc;
using EntraIDScanner.API.Models;
using MongoDB.Driver;
using EntraIDScanner.API.RepositoryInterfaces.StoredDatabaseRepository;

namespace EntraIDScanner.API.Controllers.Users.DataBaseUsers
{
    public class MongoDBUserController : Controller
    {
        private readonly IStoredDatabaseUserRepository _db;
        private readonly ILogger<MongoDBUserController> _logger;
        public MongoDBUserController(
                IStoredDatabaseUserRepository db,
                ILogger<MongoDBUserController> log)
            {
                _db = db;
                _logger = log;
            }
        [HttpGet("allmongodbusers")]
        public async Task<IActionResult> GetUsersFromDb()
        {
            try
            {
                var users = await _db.GetAllAsync();
                if (users.Count == 0)
                {
                    _logger.LogWarning("No users found in MongoDB.");
                    return NotFound("No users in database.");
                }
                _logger.LogInformation($"Returned {users.Count} users from MongoDB.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading users from MongoDB.");
                return StatusCode(500, "Internal Server Error");
            }
        }
        [HttpGet("getsingledbuser")]
        public async Task <IActionResult> GetSingleUserFromDb([FromQuery] string identifier, [FromQuery] string table)
        {
            throw new Exception();
        }
    }
}
