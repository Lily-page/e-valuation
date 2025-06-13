using PropertyAPI.Models;

namespace PropertyAPI.Services
{
    public interface IJwtService
    {
        TokenResponse GenerateAnonymousToken();
    }
}