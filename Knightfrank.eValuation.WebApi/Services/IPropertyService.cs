using Knightfrank.eValuation.WebApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Knightfrank.eValuation.WebApi.Services;

public interface IPropertyService
{
    Task<IEnumerable<Property>> SearchPropertiesAsync(PropertySearchRequest request);
    Task<ValuationResponse?> GetValuationAsync(PropertySearchRequest request, string token);
    Task<IEnumerable<string>> GetZonesAsync();
    Task<IEnumerable<string>> GetDistrictsAsync(string? zone = null);
    Task<IEnumerable<string>> GetEstateNamesAsync(string? zone = null, string? district = null);
    Task<IEnumerable<string>> GetBlocksAsync(string? zone = null, string? district = null, string? estateName = null);
    Task<IEnumerable<string>> GetFloorsAsync(string? zone = null, string? district = null, string? estateName = null, string? block = null);
    Task<IEnumerable<string>> GetUnitsAsync(string? zone = null, string? district = null, string? estateName = null, string? block = null, string? floor = null);
}