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

    public static SeasonType CalculateSeason(this DateTime dateTime)
    {
        return dateTime > new DateTime(2022, 8, 9) && dateTime < new DateTime(2022, 8, 28) ? SeasonType.PRE
            : dateTime > new DateTime(2022, 8, 31) && dateTime < new DateTime(2023, 1, 14) ? SeasonType.REG
            : SeasonType.POST;
    }
}
