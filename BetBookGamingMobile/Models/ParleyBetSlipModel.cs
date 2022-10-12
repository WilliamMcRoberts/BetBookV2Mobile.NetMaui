
namespace BetBookGamingMobile.Models;

public class ParleyBetSlipModel
{
    public string ParleyBetSlipId { get; set; }
    public List<SingleBetForParleyModel> SingleBetsForParleyList { get; set; } = new();
    public decimal? ParleyBetAmount { get; set; }
    public decimal? ParleyBetPayout { get; set; }
    public string BettorId { get; set; }
    public ParleyBetSlipPayoutStatus ParleyBetSlipPayoutStatus { get; set; }
    public ParleyBetSlipStatus ParleyBetSlipStatus { get; set; }
}
