using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.Services
{
    public interface IParleyBetSlipService
    {
        Task<bool> CreateParleyBet(ParleyBetSlipModel parleyBet);
        Task<IEnumerable<ParleyBetSlipModel>> GetAllBettorParleyBets(string userId);
    }
}