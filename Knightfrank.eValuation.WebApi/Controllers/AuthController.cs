using Microsoft.AspNetCore.Mvc;
using PropertyAPI.Models;
using PropertyAPI.Services;

namespace PropertyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IJwtService _jwtService;

        public AuthController(IJwtService jwtService)
        {
            _jwtService = jwtService;
        }

        /// <summary>
        /// Generates an anonymous JWT token for unauthenticated access
        /// </summary>
        /// <returns>A JWT token that can be used for anonymous API access</returns>
        [HttpPost("anonymous-token")]
        public ActionResult<TokenResponse> GetAnonymousToken()
        {
            try
            {
                var tokenResponse = _jwtService.GenerateAnonymousToken();
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to generate token", error = ex.Message });
            }
        }

        /// <summary>
        /// Health check endpoint to verify the auth service is working
        /// </summary>
        /// <returns>Service status</returns>
        [HttpGet("health")]
        public ActionResult GetHealth()
        {
            return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
        }
    }
}