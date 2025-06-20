using Microsoft.EntityFrameworkCore;
using Knightfrank.eValuation.WebApi.Data;
using Knightfrank.eValuation.WebApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace Knightfrank.eValuation.WebApi.Services;

public class PropertyService : IPropertyService
{
    private readonly EValuationContext _context;
    private readonly ILogger<PropertyService> _logger;

    public PropertyService(EValuationContext context, ILogger<PropertyService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchRequest request)
    {
        try
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(request.Zone))
            {
                query = query.Where(p => p.Zone.Contains(request.Zone));
            }

            if (!string.IsNullOrEmpty(request.District))
            {
                query = query.Where(p => p.District.Contains(request.District));
            }

            if (!string.IsNullOrEmpty(request.EstateName))
            {
                query = query.Where(p => p.EstateName.Contains(request.EstateName));
            }

            if (!string.IsNullOrEmpty(request.Block))
            {
                query = query.Where(p => p.Block != null && p.Block.Contains(request.Block));
            }

            if (!string.IsNullOrEmpty(request.Floor))
            {
                query = query.Where(p => p.Floor != null && p.Floor.Contains(request.Floor));
            }

            if (!string.IsNullOrEmpty(request.Unit))
            {
                query = query.Where(p => p.Unit != null && p.Unit.Contains(request.Unit));
            }

            return await query.Include(p => p.Valuations).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error searching properties");
            return Enumerable.Empty<Property>();
        }
    }

    public async Task<ValuationResponse?> GetValuationAsync(PropertySearchRequest request, string token)
    {
        try
        {
            var properties = await SearchPropertiesAsync(request);
            var property = properties.FirstOrDefault();

            if (property == null)
            {
                _logger.LogWarning("No property found for search criteria");
                return null;
            }

            var latestValuation = property.Valuations
                .OrderByDescending(v => v.ValuationDate)
                .FirstOrDefault();

            if (latestValuation == null)
            {
                // Generate a mock valuation for demonstration
                latestValuation = GenerateMockValuation(property);

                // Save the valuation with token reference
                latestValuation.TokenUsed = token;
                _context.Valuations.Add(latestValuation);
                await _context.SaveChangesAsync();
            }

            var propertyDetails = $"{property.EstateName}";
            if (!string.IsNullOrEmpty(property.Block))
                propertyDetails += $", Block {property.Block}";
            if (!string.IsNullOrEmpty(property.Floor))
                propertyDetails += $", Floor {property.Floor}";
            if (!string.IsNullOrEmpty(property.Unit))
                propertyDetails += $", Unit {property.Unit}";

            return new ValuationResponse
            {
                EstimatedValue = latestValuation.EstimatedValue,
                GrossFloorArea = property.GrossFloorArea,
                SaleableFloorArea = property.SaleableFloorArea,
                GfaUnitRate = latestValuation.GfaUnitRate,
                SfaUnitRate = latestValuation.SfaUnitRate,
                BuiltYear = property.BuiltYear,
                ValuationDate = latestValuation.ValuationDate,
                PropertyDetails = propertyDetails
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting valuation");
            return null;
        }
    }

    public async Task<IEnumerable<string>> GetZonesAsync()
    {
        try
        {
            return await _context.Properties
                .Select(p => p.Zone)
                .Distinct()
                .OrderBy(z => z)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting zones");
            return Enumerable.Empty<string>();
        }
    }

    public async Task<IEnumerable<string>> GetDistrictsAsync(string? zone = null)
    {
        try
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(zone))
            {
                query = query.Where(p => p.Zone == zone);
            }

            return await query
                .Select(p => p.District)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting districts");
            return Enumerable.Empty<string>();
        }
    }

    public async Task<IEnumerable<string>> GetEstateNamesAsync(string? zone = null, string? district = null)
    {
        try
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(zone))
            {
                query = query.Where(p => p.Zone == zone);
            }

            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(p => p.District == district);
            }

            return await query
                .Select(p => p.EstateName)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting estate names");
            return Enumerable.Empty<string>();
        }
    }

    public async Task<IEnumerable<string>> GetBlocksAsync(string? zone = null, string? district = null, string? estateName = null)
    {
        try
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(zone))
            {
                query = query.Where(p => p.Zone == zone);
            }

            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(p => p.District == district);
            }

            if (!string.IsNullOrEmpty(estateName))
            {
                query = query.Where(p => p.EstateName == estateName);
            }

            return await query
                .Where(p => p.Block != null)
                .Select(p => p.Block!)
                .Distinct()
                .OrderBy(b => b)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting blocks");
            return Enumerable.Empty<string>();
        }
    }

    public async Task<IEnumerable<string>> GetFloorsAsync(string? zone = null, string? district = null, string? estateName = null, string? block = null)
    {
        try
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(zone))
            {
                query = query.Where(p => p.Zone == zone);
            }

            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(p => p.District == district);
            }

            if (!string.IsNullOrEmpty(estateName))
            {
                query = query.Where(p => p.EstateName == estateName);
            }

            if (!string.IsNullOrEmpty(block))
            {
                query = query.Where(p => p.Block == block);
            }

            return await query
                .Where(p => p.Floor != null)
                .Select(p => p.Floor!)
                .Distinct()
                .OrderBy(f => f)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting floors");
            return Enumerable.Empty<string>();
        }
    }

    public async Task<IEnumerable<string>> GetUnitsAsync(string? zone = null, string? district = null, string? estateName = null, string? block = null, string? floor = null)
    {
        try
        {
            var query = _context.Properties.AsQueryable();

            if (!string.IsNullOrEmpty(zone))
            {
                query = query.Where(p => p.Zone == zone);
            }

            if (!string.IsNullOrEmpty(district))
            {
                query = query.Where(p => p.District == district);
            }

            if (!string.IsNullOrEmpty(estateName))
            {
                query = query.Where(p => p.EstateName == estateName);
            }

            if (!string.IsNullOrEmpty(block))
            {
                query = query.Where(p => p.Block == block);
            }

            if (!string.IsNullOrEmpty(floor))
            {
                query = query.Where(p => p.Floor == floor);
            }

            return await query
                .Where(p => p.Unit != null)
                .Select(p => p.Unit!)
                .Distinct()
                .OrderBy(u => u)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting units");
            return Enumerable.Empty<string>();
        }
    }

    private Valuation GenerateMockValuation(Property property)
    {
        // Generate realistic valuation based on property characteristics
        var random = new Random();
        var baseRate = 15000 + random.Next(1000, 5000); // Base rate per sq ft

        var gfaRate = baseRate + random.Next(-500, 1000);
        var sfaRate = gfaRate + random.Next(1000, 3000);

        var estimatedValue = property.SaleableFloorArea.HasValue
            ? property.SaleableFloorArea.Value * sfaRate
            : property.GrossFloorArea.HasValue
                ? property.GrossFloorArea.Value * gfaRate
                : 10000000; // Default value

        return new Valuation
        {
            PropertyId = property.Id,
            EstimatedValue = estimatedValue,
            GfaUnitRate = property.GrossFloorArea.HasValue ? gfaRate : null,
            SfaUnitRate = property.SaleableFloorArea.HasValue ? sfaRate : null,
            ValuationDate = DateTime.UtcNow,
            CreatedAt = DateTime.UtcNow,
            Notes = "Auto-generated valuation based on market data"
        };
    }
}