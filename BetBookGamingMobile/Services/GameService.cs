using BetBookGamingMobile.Dto;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;


namespace BetBookGamingMobile.Services;

public class GameService : IGameService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public GameService(
        IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<GameDto[]> GetGamesByWeek(
        SeasonType season, int week)
    {
        GameDto[] games = new GameDto[16];

        try
        {
            var client = _httpClientFactory.CreateClient("sportsdata");

            games = await client.GetFromJsonAsync<GameDto[]>(
                    $"scores/json/ScoresByWeek/2022{season}/{week}?key=818e3e1b20d944cc82a2b0c5c299f40d");
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return games!;
    }
}
