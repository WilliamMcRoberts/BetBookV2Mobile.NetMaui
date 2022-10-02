using Microsoft.Identity.Client;

namespace BetBookGamingMobile.Auth;

public static class Constants
{
    public static readonly string ClientId = "4e10578d-7510-45fc-bbee-87014184cae5";
    public static readonly string[] Scopes = new string[] { "openid", "offline_access" };
    //Uncomment the next code to add B2C
    public static readonly string TenantName = "betbookgamingauthentication";
    public static readonly string TenantId = $"{TenantName}.onmicrosoft.com";
    public static readonly string SignInPolicy = "B2C_1_susi"; // Or B2C_1_susi B2C_1_client
    public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
    public static readonly string AuthoritySignIn = $"{AuthorityBase}{SignInPolicy}";
    //public IPublicClientApplication PublicClientApp { get; set; }
}
