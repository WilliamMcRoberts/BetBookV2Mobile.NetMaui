

namespace BetBookGamingMobile.Models;

#nullable enable

public class SingleBetModel
{

    public string SingleBetId { get; set; } = String.Empty;

    public string BettorId { get; set; } = String.Empty;
    public decimal? BetAmount { get; set; }
    public decimal? BetPayout { get; set; }
    public decimal PointsAfterSpread { get; set; }
    public GameSnapshotModel GameSnapshot { get; set; } = new();

    public BetType BetType { get; set; }

    public SingleBetStatus SingleBetStatus { get; set; }

    public SingleBetPayoutStatus SingleBetPayoutStatus { get; set; }

    public string WinnerChosen { get; set; } = string.Empty;

    public string? BetAmountDisplay
    {
        get => $"{BetAmount:C}";
    }

    public string? BetPayoutDisplay
    {
        get => $"{BetPayout:C}";
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