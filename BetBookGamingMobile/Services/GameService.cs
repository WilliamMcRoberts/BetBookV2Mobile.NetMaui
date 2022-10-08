using BetBookGamingMobile.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Maui.Networking;
using System.Net.Http.Json;

namespace BetBookGamingMobile.Services;

public class GameService : BaseService, IGameService
{
    public GameService(IConnectivity connectivity) : base(connectivity)
    {
        SetBaseURL(Constants.GameServiceURL);
    }

    public async Task<IEnumerable<GameDto>> GetGames(SeasonType season, int week)
    {
        var resourceUri = $"2022{season}/{week}?key={Constants.GamesApiKey}";

        var result = await GetAsync<IEnumerable<GameDto>>(resourceUri);

        return result;
    }
}

