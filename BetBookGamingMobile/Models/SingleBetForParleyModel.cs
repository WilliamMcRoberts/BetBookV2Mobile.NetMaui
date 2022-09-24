


namespace BetBookGamingMobile.Models;

#nullable enable

public class SingleBetForParleyModel
{

    public string SingleBetForParleyId { get; set; }

    public BetType BetType { get; set; }

    public SingleBetForParleyStatus SingleBetForParleyStatus { get; set; }

    public GameSnapshotModel GameSnapshot { get; set; }
    public string BettorId { get; set; }
    public string WinnerChosen { get; set; } = string.Empty;
    public decimal PointsAfterSpread { get; set; }
}

#nullable disable