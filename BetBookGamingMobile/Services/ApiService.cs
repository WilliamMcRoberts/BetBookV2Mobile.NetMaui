
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
            Debug.WriteLine(ex.Message);
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
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> CreateUser(UserModel user)
    {
        try
        {
            await PostAsync<UserModel>("Users", user);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<IEnumerable<ParleyBetSlipModel>> GetAllBettorParleyBets(string userId)
    {
        var resourceUri = $"ParleyBetSlips/BettorId/{userId}";
        try
        {
            return await GetAsync<IEnumerable<ParleyBetSlipModel>>(resourceUri);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return await Task.FromResult(Enumerable.Empty<ParleyBetSlipModel>());
        }
    }

    public async Task<IEnumerable<SingleBetModel>> GetAllBettorSingleBets(string userId)
    {
        var resourceUri = $"SingleBets/BettorId/{userId}";
        try
        {
            return await GetAsync<IEnumerable<SingleBetModel>>(resourceUri);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return await Task.FromResult(Enumerable.Empty<SingleBetModel>());
        }
    }

    public async Task<IEnumerable<GameDto>> GetGames(SeasonType season, int week)
    { 
        IEnumerable<GameDto> games = new List<GameDto>();

        SetBaseURL(Constants.GameServiceURL);

        var resourceUri = $"2022{season}/{week}?key={Constants.GamesApiKey}";
        try
        {
            return await GetAsync<IEnumerable<GameDto>>(resourceUri);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
        }
        finally
        {
            SetBaseURL(Constants.VortexURL);
        }
        return games;
    }

    public async Task<UserModel> GetUserByObjectId(string objectId)
    {
        var resourceUri = $"Users/ObjectId/{objectId}";
        try
        {
            return await GetAsync<UserModel>(resourceUri);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserModel();
        }
    }

    public async Task<UserModel> GetUserByUserId(string userId)
    {
        var resourceUri = $"Users/UserId/{userId}";
        try
        {
            return await GetAsync<UserModel>(resourceUri);
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return new UserModel();
        }
    }

    public async Task<bool> UpdateUser(UserModel user)
    {
        try
        {
            await PutAsync<UserModel>("Users", user);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }
}
