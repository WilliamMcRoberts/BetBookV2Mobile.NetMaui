using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.Services
{
    public interface ISingleBetService
    {
        Task<bool> CreateSingleBet(SingleBetModel singleBet);
        Task<IEnumerable<SingleBetModel>> GetAllBettorSingleBets(string userId);
    }
}