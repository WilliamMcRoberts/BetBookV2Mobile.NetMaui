
namespace BetBookGamingMobile.Services;

public class ApiService : BaseService, IApiService
{

    public ApiService(IConnectivity connectivity) : base(connectivity)
    {
        SetBaseURL(Constants.VortexURL);
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

    public async Task<bool> CreateUser(UserModel user)
    {
        await PostAsync<UserModel>("Users", user);

        return true;
    }

    public async Task<IEnumerable<ParleyBetSlipModel>> GetAllBettorParleyBets(string userId)
    {
        var resourceUri = $"ParleyBetSlips/BettorId={userId}";

        return await GetAsync<IEnumerable<ParleyBetSlipModel>>(resourceUri);
    }

    public async Task<IEnumerable<SingleBetModel>> GetAllBettorSingleBets(string userId)
    {
        var resourceUri = $"SingleBets/BettorId={userId}";

        return await GetAsync<IEnumerable<SingleBetModel>>(resourceUri);
    }

    public async Task<IEnumerable<GameDto>> GetGames(SeasonType season, int week)
    {
        SetBaseURL(Constants.GameServiceURL);
        var resourceUri = $"2022{season}/{week}?key={Constants.GamesApiKey}";
        
        var result = await GetAsync<IEnumerable<GameDto>>(resourceUri);
        SetBaseURL(Constants.VortexURL);

        return result;
    }

    public async Task<UserModel> GetUserByObjectId(string objectId)
    {
        var user = new UserModel();
        try
        {
            var resourceUri = $"Users/ObjectId/{objectId}";

            user = await GetAsync<UserModel>(resourceUri);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return user;
    }

    public async Task<UserModel> GetUserByUserId(string userId)
    {
        var user = new UserModel();
        try
        {
            var resourceUri = $"Users/UserId/{userId}";

            user = await GetAsync<UserModel>(resourceUri);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return user;
    }

    public async Task<bool> UpdateUser(UserModel user)
    {
        await PutAsync<UserModel>("Users", user);

        return true;
    }
}
