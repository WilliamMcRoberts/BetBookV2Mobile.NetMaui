namespace BetBookGamingMobile.Services
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetGames(SeasonType season, int week);
    }
}