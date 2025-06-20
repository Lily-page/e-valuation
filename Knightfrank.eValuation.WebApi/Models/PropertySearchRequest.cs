using System;

namespace Knightfrank.eValuation.WebApi.Models;

public class PropertySearchRequest
{
    public string? Zone { get; set; }
    public string? District { get; set; }
    public string? EstateName { get; set; }
    public string? Block { get; set; }
    public string? Floor { get; set; }
    public string? Unit { get; set; }
}

public class TokenRequest
{
    public string? SessionId { get; set; }
}

public class TokenResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public string SessionId { get; set; } = string.Empty;
}

public class ValuationResponse
{
    public decimal EstimatedValue { get; set; }
    public decimal? GrossFloorArea { get; set; }
    public decimal? SaleableFloorArea { get; set; }
    public decimal? GfaUnitRate { get; set; }
    public decimal? SfaUnitRate { get; set; }
    public DateTime? BuiltYear { get; set; }
    public DateTime ValuationDate { get; set; }
    public string PropertyDetails { get; set; } = string.Empty;
}