using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PropertyAPI.Data;
using PropertyAPI.Models;

namespace PropertyAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PropertiesController : ControllerBase
    {
        private readonly PropertyContext _context;

        public PropertiesController(PropertyContext context)
        {
            _context = context;
        }

        [HttpPost("search")]
        public async Task<ActionResult<PagedResult<Property>>> SearchProperties(PropertySearchRequest request)
        {
            var query = _context.Properties.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Region))
            {
                query = query.Where(p => p.Region == request.Region);
            }

            if (!string.IsNullOrEmpty(request.District))
            {
                query = query.Where(p => p.District == request.District);
            }

            if (!string.IsNullOrEmpty(request.PropertyType))
            {
                query = query.Where(p => p.PropertyType == request.PropertyType);
            }

            if (request.MinPrice.HasValue)
            {
                query = query.Where(p => p.SalePrice >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(p => p.SalePrice <= request.MaxPrice.Value);
            }

            // Get total count
            var totalCount = await query.CountAsync();

            // Apply pagination
            var properties = await query
                .OrderBy(p => p.Id)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var result = new PagedResult<Property>
            {
                Data = properties,
                TotalCount = totalCount,
                Page = request.Page,
                PageSize = request.PageSize
            };

            return Ok(result);
        }

        [HttpGet("filters")]
        public async Task<ActionResult<object>> GetFilterOptions()
        {
            var regions = await _context.Properties
                .Select(p => p.Region)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync();

            var districts = await _context.Properties
                .Select(p => p.District)
                .Distinct()
                .OrderBy(d => d)
                .ToListAsync();

            var propertyTypes = await _context.Properties
                .Select(p => p.PropertyType)
                .Distinct()
                .OrderBy(pt => pt)
                .ToListAsync();

            var priceRanges = new[]
            {
                new { Label = "Under $300,000", Min = 0, Max = 300000 },
                new { Label = "$300,000 - $500,000", Min = 300000, Max = 500000 },
                new { Label = "$500,000 - $800,000", Min = 500000, Max = 800000 },
                new { Label = "$800,000 - $1,200,000", Min = 800000, Max = 1200000 },
                new { Label = "Over $1,200,000", Min = 1200000, Max = int.MaxValue }
            };

            return Ok(new
            {
                Regions = regions,
                Districts = districts,
                PropertyTypes = propertyTypes,
                PriceRanges = priceRanges
            });
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Property>> GetProperty(int id)
        {
            var property = await _context.Properties.FindAsync(id);

            if (property == null)
            {
                return NotFound();
            }

            return property;
        }
    }
}