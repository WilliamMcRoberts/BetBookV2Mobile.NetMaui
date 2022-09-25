using BetBookGamingMobile.Dto;

namespace BetBookGamingMobile.Services
{
    public interface IGameService
    {
        Task<GameDto[]> GetGamesByWeek(SeasonType season, int week);
    }
}