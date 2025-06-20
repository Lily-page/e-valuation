using Microsoft.AspNetCore.Mvc;
using Knightfrank.eValuation.WebApi.Models;
using Knightfrank.eValuation.WebApi.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Knightfrank.eValuation.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController : ControllerBase
{
    private readonly IPropertyService _propertyService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<PropertyController> _logger;

    public PropertyController(
        IPropertyService propertyService,
        ITokenService tokenService,
        ILogger<PropertyController> logger)
    {
        _propertyService = propertyService;
        _tokenService = tokenService;
        _logger = logger;
    }

    /// <summary>
    /// Get property valuation (requires valid anonymous token)
    /// </summary>
    [HttpPost("valuation")]
    public async Task<ActionResult<ValuationResponse>> GetValuation(
        [FromBody] PropertySearchRequest request,
        [FromHeader(Name = "X-Anonymous-Token")] string? token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Anonymous token is required" });
            }

            var isValidToken = await _tokenService.ValidateTokenAsync(token);
            if (!isValidToken)
            {
                return Unauthorized(new { message = "Invalid or expired token" });
            }

            var valuation = await _propertyService.GetValuationAsync(request, token);

            if (valuation == null)
            {
                return NotFound(new { message = "No property found matching the search criteria" });
            }

            _logger.LogInformation("Valuation requested for property with token {Token}", token);

            return Ok(valuation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting valuation");
            return StatusCode(500, new { message = "Error processing valuation request" });
        }
    }

    /// <summary>
    /// Search properties (requires valid anonymous token)
    /// </summary>
    [HttpPost("search")]
    public async Task<ActionResult<IEnumerable<Property>>> SearchProperties(
        [FromBody] PropertySearchRequest request,
        [FromHeader(Name = "X-Anonymous-Token")] string? token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Anonymous token is required" });
            }

            var isValidToken = await _tokenService.ValidateTokenAsync(token);
            if (!isValidToken)
            {
                return Unauthorized(new { message = "Invalid or expired token" });
            }

            var properties = await _propertyService.SearchPropertiesAsync(request);

            return Ok(properties);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching properties");
            return StatusCode(500, new { message = "Error searching properties" });
        }
    }

    /// <summary>
    /// Get available zones (public endpoint)
    /// </summary>
    [HttpGet("zones")]
    public async Task<ActionResult<IEnumerable<string>>> GetZones()
    {
        try
        {
            var zones = await _propertyService.GetZonesAsync();
            return Ok(zones);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting zones");
            return StatusCode(500, new { message = "Error getting zones" });
        }
    }

    /// <summary>
    /// Get available districts (public endpoint)
    /// </summary>
    [HttpGet("districts")]
    public async Task<ActionResult<IEnumerable<string>>> GetDistricts([FromQuery] string? zone = null)
    {
        try
        {
            var districts = await _propertyService.GetDistrictsAsync(zone);
            return Ok(districts);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting districts");
            return StatusCode(500, new { message = "Error getting districts" });
        }
    }

    /// <summary>
    /// Get available estate names (public endpoint)
    /// </summary>
    [HttpGet("estates")]
    public async Task<ActionResult<IEnumerable<string>>> GetEstateNames(
        [FromQuery] string? zone = null,
        [FromQuery] string? district = null)
    {
        try
        {
            var estates = await _propertyService.GetEstateNamesAsync(zone, district);
            return Ok(estates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting estate names");
            return StatusCode(500, new { message = "Error getting estate names" });
        }
    }

    /// <summary>
    /// Get available blocks (public endpoint)
    /// </summary>
    [HttpGet("blocks")]
    public async Task<ActionResult<IEnumerable<string>>> GetBlocks(
        [FromQuery] string? zone = null,
        [FromQuery] string? district = null,
        [FromQuery] string? estateName = null)
    {
        try
        {
            var blocks = await _propertyService.GetBlocksAsync(zone, district, estateName);
            return Ok(blocks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting blocks");
            return StatusCode(500, new { message = "Error getting blocks" });
        }
    }

    /// <summary>
    /// Get available floors (public endpoint)
    /// </summary>
    [HttpGet("floors")]
    public async Task<ActionResult<IEnumerable<string>>> GetFloors(
        [FromQuery] string? zone = null,
        [FromQuery] string? district = null,
        [FromQuery] string? estateName = null,
        [FromQuery] string? block = null)
    {
        try
        {
            var floors = await _propertyService.GetFloorsAsync(zone, district, estateName, block);
            return Ok(floors);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting floors");
            return StatusCode(500, new { message = "Error getting floors" });
        }
    }

    /// <summary>
    /// Get available units (public endpoint)
    /// </summary>
    [HttpGet("units")]
    public async Task<ActionResult<IEnumerable<string>>> GetUnits(
        [FromQuery] string? zone = null,
        [FromQuery] string? district = null,
        [FromQuery] string? estateName = null,
        [FromQuery] string? block = null,
        [FromQuery] string? floor = null)
    {
        try
        {
            var units = await _propertyService.GetUnitsAsync(zone, district, estateName, block, floor);
            return Ok(units);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting units");
            return StatusCode(500, new { message = "Error getting units" });
        }
    }
}