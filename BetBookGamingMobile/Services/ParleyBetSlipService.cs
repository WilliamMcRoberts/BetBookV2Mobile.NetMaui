
namespace BetBookGamingMobile.Services;

public class ParleyBetSlipService : BaseService, IParleyBetSlipService
{

    public ParleyBetSlipService(IConnectivity connectivity) :base(connectivity)
    {
        SetBaseURL(Constants.VortexURL);
    }

    public async Task<IEnumerable<ParleyBetSlipModel>> GetAllBettorParleyBets(string userId)
    {
        var resourceUri = $"ParleyBetSlips/BettorId={userId}";

        return await GetAsync<IEnumerable<ParleyBetSlipModel>>(resourceUri);
    }

    public async Task<bool> CreateParleyBet(ParleyBetSlipModel parleyBet)
    {
        try
        {
            await PostAsync<ParleyBetSlipModel>("ParleyBetSlips", parleyBet);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
        
    }
}
