
namespace BetBookGamingMobile.Services;

public class SingleBetService : BaseService, ISingleBetService
{
    public SingleBetService(IConnectivity connectivity) :base(connectivity)
    {
        SetBaseURL(Constants.VortexURL);
    }

    public async Task<IEnumerable<SingleBetModel>> GetAllBettorSingleBets(string userId)
    {
        var resourceUri = $"SingleBets/BettorId={userId}";

        return await GetAsync<IEnumerable<SingleBetModel>>(resourceUri);
    }

    public async Task<bool> CreateSingleBet(SingleBetModel singleBet)
    {
        try
        {
            await PostAsync<SingleBetModel>("SingleBets", singleBet);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }

    }
}
