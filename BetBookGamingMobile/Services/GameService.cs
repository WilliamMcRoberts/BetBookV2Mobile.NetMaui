using BetBookGamingMobile.Dto;
using System.Net.Http.Json;


namespace BetBookGamingMobile.Services;

public class GameService : IGameService
{

    private readonly IHttpClientFactory _httpClientFactory;

    public GameService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<GameDto[]> GetGamesByWeekAndSeason(
        SeasonType season, int week)
    {
        GameDto[] games = new GameDto[16];

        try
        {
            var client = _httpClientFactory.CreateClient("vortex");

            games = await client.GetFromJsonAsync<GameDto[]>($"Games/{season}/{week}");
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return games;
    }
}
