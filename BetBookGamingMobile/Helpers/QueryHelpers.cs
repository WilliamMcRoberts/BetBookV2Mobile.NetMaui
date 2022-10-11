

namespace BetBookGamingMobile.Helpers;

public static class QueryHelpers
{
    public static int GetMoneylinePayoutForBet(this string winner, GameDto game, BetType betType) =>
        betType == BetType.POINTSPREAD ?
        (winner == game.AwayTeam ? (int)game.PointSpreadAwayTeamMoneyLine! : (int)game.PointSpreadHomeTeamMoneyLine!)
        : betType == BetType.OVERUNDER ? (winner[0] == 'O' ? (int)game.OverPayout! : (int)game.UnderPayout!)
        : (winner == game.AwayTeam ? (int)game.AwayTeamMoneyLine! : (int)game.HomeTeamMoneyLine!);

    public static string GetWinnerSummary(this CreateBetModel createBetModel) =>
        createBetModel.BetType == BetType.POINTSPREAD ? GetWinnerSummaryForPointSpread(createBetModel)
        : createBetModel.BetType == BetType.OVERUNDER ? GetWinnerSummaryForOverUnder(createBetModel)
        : createBetModel.Winner;

    public static string GetWinnerSummaryForOverUnder(this CreateBetModel createBetModel) =>
         createBetModel.Winner[0] == 'O' ? $"Over {createBetModel.Game.OverUnder}"
         : $"Under {createBetModel.Game.OverUnder}";

    public static string GetWinnerSummaryForPointSpread(this CreateBetModel createBetModel) =>
         createBetModel.Winner == createBetModel.Game.HomeTeam ?
         $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread):+#.0;-#.0}"
         : $"{createBetModel.Winner} {Convert.ToDecimal(createBetModel.Game.PointSpread):-#.0;+#.0;}";
}
