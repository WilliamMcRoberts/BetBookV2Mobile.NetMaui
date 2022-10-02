using Microsoft.Identity.Client;

namespace BetBookGamingMobile.Auth
{
    public interface IAuthService
    {
        Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken);
    }
}