
namespace BetBookGamingMobile.Auth;

public static class Constants
{
    public static readonly string ClientId = "";
    public static readonly string[] Scopes = new string[] { "openid", "offline_access" };
    public static readonly string GamesApiKey = "";
    public static readonly string GameServiceURL = "https://api.sportsdata.io/v3/nfl/scores/json/ScoresByWeek/";
    public static readonly string BetBookGamingV2URL = "https://betbookgamingv2api.azurewebsites.net/";
    public static readonly string BetBookGamingApiKey = "~";
    public static readonly string B2CSigninSignupAuthority = "https://betbookgamingauthentication.b2clogin.com/tfp/betbookgamingauthentication.onmicrosoft.com/B2C_1_susi";
}
