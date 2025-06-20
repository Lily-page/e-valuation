using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace Knightfrank.eValuation.WebApi.Models;

public class Property
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Zone { get; set; } = string.Empty;

    [Required]
    public string District { get; set; } = string.Empty;

    [Required]
    public string EstateName { get; set; } = string.Empty;

    public string? Block { get; set; }

    public string? Floor { get; set; }

    public string? Unit { get; set; }

    public decimal? GrossFloorArea { get; set; }

    public decimal? SaleableFloorArea { get; set; }

    public DateTime? BuiltYear { get; set; }

    public virtual ICollection<Valuation> Valuations { get; set; } = new List<Valuation>();
}