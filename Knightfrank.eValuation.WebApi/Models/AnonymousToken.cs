using System.ComponentModel.DataAnnotations;

namespace Knightfrank.eValuation.WebApi.Models;

public class AnonymousToken
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Token { get; set; } = string.Empty;

    [Required]
    public DateTime CreatedAt { get; set; }

    [Required]
    public DateTime ExpiresAt { get; set; }

    public bool IsExpired => DateTime.UtcNow > ExpiresAt;

    public string? SessionId { get; set; }

    public int RequestCount { get; set; } = 0;

    public DateTime? LastUsedAt { get; set; }
}