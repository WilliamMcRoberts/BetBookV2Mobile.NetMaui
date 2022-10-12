
namespace BetBookGamingMobile.Auth;

public static class Constants
{
    public static readonly string ClientId = "4e10578d-7510-45fc-bbee-87014184cae5";
    public static readonly string[] Scopes = new string[] { "openid", "offline_access" };
    public static readonly string TenantName = "betbookgamingauthentication";
    public static readonly string TenantId = $"{TenantName}.onmicrosoft.com";
    public static readonly string SignInPolicy = "B2C_1_susi";
    public static readonly string EditPolicy = "B2C_1_edit";
    public static readonly string ResetPolicy = "B2C_1_reset";
    public static readonly string AuthorityBase = $"https://{TenantName}.b2clogin.com/tfp/{TenantId}/";
    public static readonly string AuthoritySignIn = $"{AuthorityBase}{SignInPolicy}";
    public static readonly string AuthorityReset = $"{AuthorityBase}{ResetPolicy}";
    public static readonly string AuthorityEdit = $"{AuthorityBase}{EditPolicy}";
    public static string GamesApiKey = "818e3e1b20d944cc82a2b0c5c299f40d";
    public static string GameServiceURL = "https://api.sportsdata.io/v3/nfl/scores/json/ScoresByWeek/";
    public static string VortexURL = "https://user9f9bd262219b696.app.vtxhub.com/";
    public static string BetBookGamingApiKey = "~";
}
