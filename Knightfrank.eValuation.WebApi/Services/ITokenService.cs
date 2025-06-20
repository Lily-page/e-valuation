using Knightfrank.eValuation.WebApi.Models;
using System.Threading.Tasks;

namespace Knightfrank.eValuation.WebApi.Services;

public interface ITokenService
{
	Task<TokenResponse> GenerateAnonymousTokenAsync(string? sessionId = null);
	Task<bool> ValidateTokenAsync(string token);
	Task<AnonymousToken?> GetTokenAsync(string token);
	Task<bool> RefreshTokenAsync(string token);
	Task CleanupExpiredTokensAsync();
}