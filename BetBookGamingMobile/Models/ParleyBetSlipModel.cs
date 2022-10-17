
namespace BetBookGamingMobile.Models;
#nullable enable
public class ParleyBetSlipModel
{
    public string ParleyBetSlipId { get; set; } = String.Empty;
    public List<SingleBetForParleyModel> SingleBetsForParleyList { get; set; } = new();
    public decimal? ParleyBetAmount { get; set; }
    public decimal? ParleyBetPayout { get; set; }
    public string BettorId { get; set; } = String.Empty;
    public ParleyBetSlipPayoutStatus ParleyBetSlipPayoutStatus { get; set; }
    public ParleyBetSlipStatus ParleyBetSlipStatus { get; set; }

    public string? ParleyBetAmountDisplay
    {
        get => $"{ParleyBetAmount:C}";
    }

    public string? ParleyBetPayoutDisplay
    {
        get => $"{ParleyBetPayout:C}";
    }
}

#nullable disable
