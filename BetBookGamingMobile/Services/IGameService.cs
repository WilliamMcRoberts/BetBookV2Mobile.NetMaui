using BetBookGamingMobile.Dto;

namespace BetBookGamingMobile.Services
{
    public interface IGameService
    {
        Task<GameDto[]> GetGamesByWeekAndSeason(int week, SeasonType season);
    }
}