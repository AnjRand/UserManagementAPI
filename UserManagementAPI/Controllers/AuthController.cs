using Microsoft.AspNetCore.Mvc;
using UserManagementAPI.Services;

namespace UserManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;
        private readonly IWebHostEnvironment _env;

        public AuthController(
            ITokenService tokenService,
            IConfiguration configuration,
            ILogger<AuthController> logger,
            IWebHostEnvironment env)
        {
            _tokenService = tokenService;
            _configuration = configuration;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Generate a test token (Development only)
        /// </summary>
        /// <returns>Bearer token for testing</returns>
        [HttpPost("token")]
        public IActionResult GenerateTestToken()
        {
            if (!_env.IsDevelopment())
            {
                return Forbid("This endpoint is only available in Development environment");
            }

            try
            {
                var token = _tokenService.GenerateToken("test-user", "Test User");
                return Ok(new 
                { 
                    token, 
                    type = "Bearer", 
                    expiresIn = "1 hour",
                    message = "Use this token in the Authorization header or the Authorize button in Swagger",
                    usage = "Authorization: Bearer {token}"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token generation failed");
                return StatusCode(500, new { error = "Token generation failed" });
            }
        }

        /// <summary>
        /// Login with API Key to get a bearer token
        /// </summary>
        /// <param name="apiKey">Your API Key in the X-API-Key header</param>
        /// <returns>Bearer token for subsequent requests</returns>
        [HttpPost("login")]
        public IActionResult Login([FromHeader(Name = "X-API-Key")] string apiKey)
        {
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                return BadRequest(new { error = "API Key is required in X-API-Key header" });
            }

            var validApiKeys = _configuration.GetSection("Authentication:ApiKeys").Get<string[]>() ?? Array.Empty<string>();

            if (!validApiKeys.Contains(apiKey))
            {
                _logger.LogWarning("Login attempt with invalid API Key");
                return Unauthorized(new { error = "Invalid API Key" });
            }

            try
            {
                var token = _tokenService.GenerateToken("api-user", "API User");
                return Ok(new { token, type = "Bearer", expiresIn = "1 hour" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token generation failed");
                return StatusCode(500, new { error = "Token generation failed" });
            }
        }

        /// <summary>
        /// Verify that the bearer token is valid
        /// </summary>
        /// <returns>Verification status</returns>
        [HttpGet("verify")]
        public IActionResult Verify()
        {
            if (User?.Identity?.IsAuthenticated == true)
            {
                return Ok(new { message = "Token is valid", user = User.Identity.Name });
            }

            return Unauthorized(new { error = "Invalid or missing bearer token" });
        }
    }
}
