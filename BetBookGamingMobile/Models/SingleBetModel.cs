


using BetBookGamingMobile.Dto;

namespace BetBookGamingMobile.Models;

#nullable enable

public class SingleBetModel
{

    public string SingleBetId { get; set; }

    public string BettorId { get; set; }
    public decimal BetAmount { get; set; }
    public decimal BetPayout { get; set; }
    public decimal PointsAfterSpread { get; set; }
    public GameSnapshotModel GameSnapshot { get; set; }

    public BetType BetType { get; set; }

    public SingleBetStatus SingleBetStatus { get; set; }

    public SingleBetPayoutStatus SingleBetPayoutStatus { get; set; }

    public string WinnerChosen { get; set; } = string.Empty;
}

#nullable disable