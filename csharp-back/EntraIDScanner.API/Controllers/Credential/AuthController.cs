using Microsoft.AspNetCore.Mvc;

namespace EntraIDScanner.API.Controllers.Credential
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        public IActionResult SetCredentials([FromBody] AuthModel authData)
        {
            if (authData == null || string.IsNullOrEmpty(authData.TenantId) ||
                string.IsNullOrEmpty(authData.ClientId) || string.IsNullOrEmpty(authData.ClientSecret))
            {
                return BadRequest("Invalid authentication data.");
            }
            return Ok(new { message = "Azure credentials updated successfully." });
        }
    }

    public class AuthModel
    {
        public string TenantId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}