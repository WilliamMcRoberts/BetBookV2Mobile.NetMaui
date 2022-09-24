
using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.Services
{
    public interface ISingleBetService
    {
        Task CreateSingleBet(SingleBetModel singleBet);
    }
}