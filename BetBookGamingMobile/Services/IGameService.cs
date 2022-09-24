
using BetBookGamingMobile.Dto;

namespace BetBookGamingMobile.Services
{
    public interface IGameService
    {
        Task<GameDto[]> GetGamesByWeekAndSeason(SeasonType season, int week);
    }
}