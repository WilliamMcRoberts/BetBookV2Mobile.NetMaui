

namespace BetBookGamingMobile.Models;

public class BetModeStateModel
{
    public bool SingleInProgress { get; set; }
    public bool SingleWinners{ get; set; }
    public bool SingleLosers { get; set; }
    public bool SinglePush { get; set; }
    public bool ParleyInProgress { get; set; }
    public bool ParleyWinners { get; set; }
    public bool ParleyLosers { get; set; }
    public bool ParleyPush { get; set; }
}
