


namespace BetBookGamingMobile.Models;

#nullable enable

public class SingleBetForParleyModel
{

    public string SingleBetForParleyId { get; set; } = String.Empty;

    public BetType BetType { get; set; }

    public SingleBetForParleyStatus SingleBetForParleyStatus { get; set; }

    public GameSnapshotModel GameSnapshot { get; set; } = new();
    public string BettorId { get; set; } = String.Empty;
    public string WinnerChosen { get; set; } = String.Empty;
    public decimal PointsAfterSpread { get; set; }

    public string? BetStatusDisplay
    {
        get => SingleBetForParleyStatus == SingleBetForParleyStatus.IN_PROGRESS ? "Active"
            : SingleBetForParleyStatus == SingleBetForParleyStatus.WINNER ? "Winner"
            : SingleBetForParleyStatus == SingleBetForParleyStatus.LOSER ? "Loser"
            : "Push";
    }

    public string? BetTypeDisplay
    {
        get => BetType == BetType.POINTSPREAD ? "PS"
            : BetType == BetType.OVERUNDER ? "OU"
            : "ML";
    }

    public string? WinnerSummary
    {
        get => BetType == BetType.POINTSPREAD ? (WinnerChosen == GameSnapshot.HomeTeam ?
         $"{WinnerChosen} {Convert.ToDecimal(GameSnapshot.PointSpread):+#.0;-#.0}"
         : $"{WinnerChosen} {Convert.ToDecimal(GameSnapshot.PointSpread):-#.0;+#.0;}") :
            BetType == BetType.OVERUNDER ? (WinnerChosen == "Over" ? $"Over {GameSnapshot.OverUnder}"
         : $"Under {GameSnapshot.OverUnder}") : WinnerChosen;
    }
}

#nullable disable