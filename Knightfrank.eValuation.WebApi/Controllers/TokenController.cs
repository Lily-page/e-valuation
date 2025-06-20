using Microsoft.AspNetCore.Mvc;
using Knightfrank.eValuation.WebApi.Models;
using Knightfrank.eValuation.WebApi.Services;
using System.Threading.Tasks;
using System;

namespace Knightfrank.eValuation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TokenController : ControllerBase
{
    private readonly ITokenService _tokenService;
    private readonly ILogger<TokenController> _logger;

    public TokenController(ITokenService tokenService, ILogger<TokenController> logger)
    {
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Generate a new anonymous token for API access
    /// </summary>
    [HttpPost("generate")]
    public async Task<ActionResult<TokenResponse>> GenerateToken([FromBody] TokenRequest? request = null)
    {
        try
        {
            var tokenResponse = await _tokenService.GenerateAnonymousTokenAsync(request?.SessionId);

            _logger.LogInformation("Generated token for session {SessionId}", tokenResponse.SessionId);

            return Ok(tokenResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating token");
            return StatusCode(500, new { message = "Error generating token" });
        }
    }

    /// <summary>
    /// Validate an existing token
    /// </summary>
    [HttpPost("validate")]
    public async Task<ActionResult<bool>> ValidateToken([FromBody] string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            var isValid = await _tokenService.ValidateTokenAsync(token);
            return Ok(new { isValid });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token");
            return StatusCode(500, new { message = "Error validating token" });
        }
    }

    /// <summary>
    /// Refresh an existing token to extend its expiration
    /// </summary>
    [HttpPost("refresh")]
    public async Task<ActionResult> RefreshToken([FromBody] string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                return BadRequest(new { message = "Token is required" });
            }

            var refreshed = await _tokenService.RefreshTokenAsync(token);

            if (!refreshed)
            {
                return NotFound(new { message = "Token not found or cannot be refreshed" });
            }

            return Ok(new { message = "Token refreshed successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token");
            return StatusCode(500, new { message = "Error refreshing token" });
        }
    }

    /// <summary>
    /// Get token information
    /// </summary>
    [HttpGet("info/{token}")]
    public async Task<ActionResult> GetTokenInfo(string token)
    {
        try
        {
            var tokenInfo = await _tokenService.GetTokenAsync(token);

            if (tokenInfo == null)
            {
                return NotFound(new { message = "Token not found" });
            }

            return Ok(new
            {
                sessionId = tokenInfo.SessionId,
                createdAt = tokenInfo.CreatedAt,
                expiresAt = tokenInfo.ExpiresAt,
                requestCount = tokenInfo.RequestCount,
                lastUsedAt = tokenInfo.LastUsedAt,
                isExpired = tokenInfo.IsExpired
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting token info");
            return StatusCode(500, new { message = "Error getting token info" });
        }
    }
}