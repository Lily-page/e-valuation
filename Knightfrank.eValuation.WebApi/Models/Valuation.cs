using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Knightfrank.eValuation.WebApi.Models;

public class Valuation
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int PropertyId { get; set; }

    [ForeignKey("PropertyId")]
    public virtual Property Property { get; set; } = null!;

    [Required]
    public decimal EstimatedValue { get; set; }

    public decimal? GfaUnitRate { get; set; }

    public decimal? SfaUnitRate { get; set; }

    [Required]
    public DateTime ValuationDate { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? Notes { get; set; }

    public string? TokenUsed { get; set; }
}