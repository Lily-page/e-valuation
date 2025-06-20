using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Knightfrank.eValuation.WebApi.Data;
using Knightfrank.eValuation.WebApi.Models;
using System.Threading.Tasks;
using System;

namespace Knightfrank.eValuation.WebApi.Services;

public class TokenService : ITokenService
{
    private readonly EValuationContext _context;
    private readonly ILogger<TokenService> _logger;
    private const int TokenExpirationHours = 24; // 24 hours expiration
    private const int MaxRequestsPerToken = 100; // Limit requests per token

    public TokenService(EValuationContext context, ILogger<TokenService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<TokenResponse> GenerateAnonymousTokenAsync(string? sessionId = null)
    {
        try
        {
            // Generate a unique token
            var token = GenerateSecureToken();
            var expiresAt = DateTime.UtcNow.AddHours(TokenExpirationHours);

            // If no sessionId provided, generate one
            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
            }

            var anonymousToken = new AnonymousToken
            {
                Token = token,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt,
                SessionId = sessionId,
                RequestCount = 0
            };

            _context.AnonymousTokens.Add(anonymousToken);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Generated new anonymous token for session {SessionId}", sessionId);

            return new TokenResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                SessionId = sessionId
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating anonymous token");
            throw;
        }
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            var anonymousToken = await _context.AnonymousTokens
                .FirstOrDefaultAsync(t => t.Token == token);

            if (anonymousToken == null)
            {
                _logger.LogWarning("Token not found: {Token}", token);
                return false;
            }

            if (anonymousToken.IsExpired)
            {
                _logger.LogWarning("Token expired: {Token}", token);
                return false;
            }

            if (anonymousToken.RequestCount >= MaxRequestsPerToken)
            {
                _logger.LogWarning("Token request limit exceeded: {Token}", token);
                return false;
            }

            // Update usage statistics
            anonymousToken.RequestCount++;
            anonymousToken.LastUsedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating token: {Token}", token);
            return false;
        }
    }

    public async Task<AnonymousToken?> GetTokenAsync(string token)
    {
        try
        {
            return await _context.AnonymousTokens
                .FirstOrDefaultAsync(t => t.Token == token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving token: {Token}", token);
            return null;
        }
    }

    public async Task<bool> RefreshTokenAsync(string token)
    {
        try
        {
            var anonymousToken = await _context.AnonymousTokens
                .FirstOrDefaultAsync(t => t.Token == token);

            if (anonymousToken == null)
            {
                return false;
            }

            // Extend expiration by another period
            anonymousToken.ExpiresAt = DateTime.UtcNow.AddHours(TokenExpirationHours);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Refreshed token: {Token}", token);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error refreshing token: {Token}", token);
            return false;
        }
    }

    public async Task CleanupExpiredTokensAsync()
    {
        try
        {
            var expiredTokens = await _context.AnonymousTokens
                .Where(t => t.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.AnonymousTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Cleaned up {Count} expired tokens", expiredTokens.Count);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning up expired tokens");
        }
    }

    private string GenerateSecureToken()
    {
        // Generate a cryptographically secure random token
        using var rng = RandomNumberGenerator.Create();
        var bytes = new byte[32]; // 256 bits
        rng.GetBytes(bytes);

        // Convert to base64 and make URL-safe
        var token = Convert.ToBase64String(bytes)
            .Replace('+', '-')
            .Replace('/', '_')
            .TrimEnd('=');

        // Add timestamp prefix for uniqueness
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        return $"anon_{timestamp}_{token}";
    }
}