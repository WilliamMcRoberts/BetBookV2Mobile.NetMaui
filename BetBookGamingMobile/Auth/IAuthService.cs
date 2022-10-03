using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile.Auth
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> GetAuthClaims();
        Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken);
    }
}