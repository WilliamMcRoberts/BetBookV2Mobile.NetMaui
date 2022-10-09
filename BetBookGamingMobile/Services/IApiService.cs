

namespace BetBookGamingMobile.Services;

public interface IApiService
{
    Task<IEnumerable<GameDto>> GetGames(SeasonType season, int week);
    Task<bool> CreateUser(UserModel user);
    Task<UserModel> GetUserByObjectId(string objectId);
    Task<UserModel> GetUserByUserId(string userId);
    Task<bool> UpdateUser(UserModel user);
    Task<bool> CreateSingleBet(SingleBetModel singleBet);
    Task<IEnumerable<SingleBetModel>> GetAllBettorSingleBets(string userId);
    Task<bool> CreateParleyBet(ParleyBetSlipModel parleyBet);
    Task<IEnumerable<ParleyBetSlipModel>> GetAllBettorParleyBets(string userId);
}
