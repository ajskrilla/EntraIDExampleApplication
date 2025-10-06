using System;
using System.Threading.Tasks;
using EntraIDScanner.API.Models;
using EntraIDScanner.API.RepositoryInterfaces.StandardInterfaces;
using EntraIDScanner.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Models;
using MongoDB.Driver;

namespace EntraIDScanner.API.Controllers.Users.EntraIdUsers
{

    [Route("api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly IUserRepository _repo;
        // This is the old file to get the stuff which is wrong
        //private readonly GraphService _graphService;
        //private readonly CredentialService _credentialService;
        // This points to the new IRepo



        public UsersController(
            IUserRepository repo,
            ILogger<UsersController> logger)
        {
            _repo = repo;
            _logger = logger;

            /// remove later
            if (_repo == null) throw new ArgumentNullException(nameof(repo));
        }
        //Health check for api
        [HttpGet("ping")]
        public IActionResult Ping() => Ok("pong");

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            try
            {
                _logger.LogInformation("UsersController initialized.");
                _logger.LogInformation("Received request to /api/users");
                // init creds and graph client
                _logger.LogInformation("init creds");
                var users = await _repo.GetAllUsersAsync();
                if (users == null || users.Count == 0)
                {
                    _logger.LogWarning("No users found in Microsoft Graph.");
                    return NotFound("No users found.");
                }

                _logger.LogInformation($"Successfully retrieved {users.Count} users.");
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users.");
                return StatusCode(500, "Internal Server Error: " + ex.Message);
            }
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> LookupUser([FromQuery] string userPrincipal)
        {
            _logger.LogInformation($"LookupUser called with id: {userPrincipal} ");
            User user = null;
            user = await _repo.GetUser(userPrincipal);  
            if (string.IsNullOrEmpty(userPrincipal))
                return BadRequest("Please provide a userprincipal");
            if (user == null)
            {
                _logger.LogWarning("No user found for the provided criteria.");
                return NotFound("User not found.");
            }
            return Ok(user);
        }
    }
}