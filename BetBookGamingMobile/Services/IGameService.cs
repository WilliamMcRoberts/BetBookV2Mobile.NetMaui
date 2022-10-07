using BetBookGamingMobile.Dto;

namespace BetBookGamingMobile.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetGamesByWeekAndSeason(int week, SeasonType season);
    }
}