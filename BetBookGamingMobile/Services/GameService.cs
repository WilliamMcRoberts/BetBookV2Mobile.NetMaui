using BetBookGamingMobile.Dto;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;


namespace BetBookGamingMobile.Services;

public class GameService : IGameService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;
    private readonly Secrets _secrets;

    public GameService(
        IHttpClientFactory httpClientFactory, IConfiguration configuration, Secrets secrets)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
        _secrets = secrets;
    }

    public async Task<GameDto[]> GetGamesByWeek(
        SeasonType season, int week)
    {
        GameDto[]? games = new GameDto[16];

        try
        {
            string key = _configuration.GetSection("Settings:Key5").Value;
            var client = _httpClientFactory.CreateClient("sportsdata");

            games = await client.GetFromJsonAsync<GameDto[]>(
                    $"scores/json/ScoresByWeek/2022{season}/{week}?key={_secrets.Key5}");
        }

        catch (Exception ex)
        {
        }

        return games!;
    }
}
