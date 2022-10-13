

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

    public static ButtonTextStateModel GetButtonTextState(this GameDto gameDto) =>
        new()
        {
            ApText = $"{gameDto.AwayTeam} {gameDto.AwayTeamPointSpreadForDisplay}    {gameDto.PointSpreadAwayTeamMoneyLine}",
            HpText = $"{gameDto.HomeTeam} {gameDto.HomeTeamPointSpreadForDisplay}    {gameDto.PointSpreadHomeTeamMoneyLine}",
            AmText = $"{gameDto.AwayTeam}   {gameDto.AwayTeamMoneyLine}",
            HmText = $"{gameDto.HomeTeam}   {gameDto.HomeTeamMoneyLine}",
            OText = $"Over {gameDto.OverUnder}    {gameDto.OverPayout}",
            UText = $"Under {gameDto.OverUnder}    {gameDto.UnderPayout}"
        };

    public static GameSnapshotModel GetGameSnapshot(this GameDto gameDto) =>
        new()
        {
            Week = gameDto.Week,
            Date = gameDto.Date,
            AwayTeam = gameDto.AwayTeam,
            HomeTeam = gameDto.HomeTeam,
            PointSpread = Math.Round(Convert.ToDecimal(gameDto.PointSpread), 1),
            OverUnder = Math.Round(Convert.ToDecimal(gameDto.OverUnder), 1),
            AwayTeamMoneyLine = gameDto.AwayTeamMoneyLine,
            HomeTeamMoneyLine = gameDto.HomeTeamMoneyLine,
            PointSpreadAwayTeamMoneyLine = gameDto.PointSpreadAwayTeamMoneyLine,
            PointSpreadHomeTeamMoneyLine = gameDto.PointSpreadHomeTeamMoneyLine,
            ScoreID = gameDto.ScoreID,
            OverPayout = gameDto.OverPayout,
            UnderPayout = gameDto.UnderPayout
        };

    public static SingleBetForParleyModel GetSingleBetForParley(this CreateBetModel bet, UserModel loggedInUser) =>
        new()
        {
            WinnerChosen = bet.BetType == BetType.OVERUNDER ?
                               (bet.Winner[0] == 'O' ? "Over" : "Under")
                                : bet.Winner,

            PointsAfterSpread =
                    bet.Game.CalculatePointsAfterSpread(bet.Winner),

            BettorId = loggedInUser.UserId,
            BetType = bet.BetType,
            SingleBetForParleyStatus = SingleBetForParleyStatus.IN_PROGRESS,
            GameSnapshot = bet.Game.GetGameSnapshot()
        };
}
