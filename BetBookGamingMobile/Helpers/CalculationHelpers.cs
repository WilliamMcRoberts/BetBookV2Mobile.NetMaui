﻿

namespace BetBookGamingMobile.Helpers;

public static class CalculationHelpers
{
    public static int CalculateWeek(this SeasonType season, DateTime dateTime)
    {
        int week = season == SeasonType.PRE ? (dateTime - new DateTime(2022, 8, 9)).Days / 7
                   : season == SeasonType.REG ? (dateTime - new DateTime(2022, 9, 6)).Days / 7
                   : (dateTime - new DateTime(2023, 1, 14)).Days / 7;

        if (week < 0)
            return 0;

        return week + 1;
    }

    public static SeasonType CalculateSeason(this DateTime dateTime) =>
        dateTime > new DateTime(2022, 8, 9) && dateTime < new DateTime(2022, 8, 28) ? SeasonType.PRE
            : dateTime > new DateTime(2022, 8, 31) && dateTime < new DateTime(2023, 1, 14) ? SeasonType.REG
            : SeasonType.POST;

    public static decimal CalculatePointsAfterSpread(this GameDto game, string chosenWinner) =>
         chosenWinner == game.HomeTeam ? 0 + (decimal)game.PointSpread!
            : 0 - (decimal)game.PointSpread!;

    public static decimal CalculateSingleBetPayout(this decimal betAmount, int moneylinePayout) =>
         moneylinePayout < 0 ? betAmount / ((decimal)moneylinePayout * -1 / 100) + betAmount
         : ((decimal)moneylinePayout / 100) * betAmount;

    public static decimal ConvertMoneylinePayoutToDecimalFormat(this int moneylinePayout) =>
         moneylinePayout < 0 ? (100 / (decimal)moneylinePayout * -1) + (decimal)1
         : ((decimal)moneylinePayout / 100) + 1;
}
