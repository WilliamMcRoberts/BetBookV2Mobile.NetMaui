

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Queries;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile.Auth;

public class AuthService : IAuthService
{
    private IPublicClientApplication _publicClientApplication;

    public AuthService()
    {
        _publicClientApplication = PublicClientApplicationBuilder.Create(Constants.ClientId)
            .WithB2CAuthority(Constants.AuthoritySignIn)

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
            result = await _publicClientApplication
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

    public async Task<JwtSecurityToken> GetAuthClaims()
    {
        var result = await LoginAsync(CancellationToken.None);
        var token = result?.IdToken;

        if (token is null) return null;

        var handler = new JwtSecurityTokenHandler();
        var data = handler.ReadJwtToken(token);

        return data;
    }
}
