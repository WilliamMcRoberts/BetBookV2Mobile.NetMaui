
namespace BetBookGamingMobile.Auth;

public static class Constants
{
    public static readonly string ClientId = "";
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
    public static string GamesApiKey = "";
    public static string GameServiceURL = "https://api.sportsdata.io/v3/nfl/scores/json/ScoresByWeek/";
    public static string BetBookGamingV2URL = "https://betbookgamingv2api.azurewebsites.net/";
    public static string BetBookGamingApiKey = "~";
}
