



using BetBookGamingLibrary;
using BetBookGamingLibrary.Dto;

namespace BetBookGamingMobile.Models;

#nullable enable

public class CreateBetModel
{
    public BetType BetType { get; set; }
    public decimal BetAmount { get; set; }
    public int MoneylinePayout { get; set; }
    public GameDto? Game { get; set; }
    public string Winner { get; set; } = String.Empty;
    public decimal? PointSpread { get; set; }
    public decimal? OverUnder { get; set; }
    public string? WinnerSummary
    { get =>
            BetType == BetType.OVERUNDER ? (Winner[0] == 'O' ? $"Over {OverUnder}" : $"Under {OverUnder}")
            : BetType == BetType.POINTSPREAD ? (Winner == Game?.AwayTeam ? $"{Game.AwayTeam} {PointSpread:-#.0;+#.0;+0}" : $"{Game?.HomeTeam} {PointSpread:+#.0;-#.0;+0}")
            : $"{Winner}";
    }
}

#nullable disable
