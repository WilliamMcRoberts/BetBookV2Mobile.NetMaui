

using BetBookGamingMobile.Helpers;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;

namespace BetBookGamingMobile.Auth;

public class AuthService : IAuthService
{

    private readonly IPublicClientApplication authenticationClient;
    public AuthService()
    {
        authenticationClient = PublicClientApplicationBuilder.Create(Constants.ClientId)
            .WithB2CAuthority(Constants.AuthoritySignIn) // uncomment to support B2C
#if WINDOWS
            .WithRedirectUri("http://localhost")
#else
            .WithRedirectUri($"msal{Constants.ClientId}://auth")
#endif
            .Build();
    }

    public async Task<AuthenticationResult> LoginAsync(CancellationToken cancellationToken)
    {
        AuthenticationResult result;
        try
        {
            result = await authenticationClient
                .AcquireTokenInteractive(Constants.Scopes)
                .WithPrompt(Prompt.ForceLogin)
#if ANDROID
                .WithParentActivityOrWindow(Platform.CurrentActivity)
#endif
#if WINDOWS
        .WithUseEmbeddedWebView(false)
#endif
                .ExecuteAsync(cancellationToken);
            return result;
        }
        catch (MsalClientException)
        {
            return null;
        }
    }
}
