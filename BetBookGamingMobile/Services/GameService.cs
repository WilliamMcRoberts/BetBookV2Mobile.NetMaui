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

    public async Task<GameDto[]> GetGamesByWeekAndSeason(
        int week, SeasonType season)
    {
        //Sample Data
        //return new GameDto[2]
        //{
        //    new GameDto
        //    {
        //        AwayTeam = "CIN", HomeTeam = "PIT", PointSpread = (float)5.5, OverUnder = (float)55.5, AwayTeamMoneyLine = 150, HomeTeamMoneyLine = -150, PointSpreadAwayTeamMoneyLine = 150, PointSpreadHomeTeamMoneyLine = -150, OverPayout = 150, UnderPayout = -150, DateTime = new DateTime(2022, 10, 2, 19, 30, 0)
        //    },
        //    new GameDto
        //    {
        //        AwayTeam = "DAL", HomeTeam = "WAS", PointSpread = (float)3.5, OverUnder = (float)33.5, AwayTeamMoneyLine = 350, HomeTeamMoneyLine = -350, PointSpreadAwayTeamMoneyLine = 350, PointSpreadHomeTeamMoneyLine = -350, OverPayout = 350, UnderPayout = -350, DateTime = new DateTime(2022, 10, 2, 19, 30, 0)
        //    }
        //};


        //GameDto[] games = new GameDto[16];

        //try
        //{
        //    var client = _httpClientFactory.CreateClient("sportsdata");

        //    games = await client.GetFromJsonAsync<GameDto[]>(
        //            $"scores/json/ScoresByWeek/2022{season}/{week}?key=");
        //}

        //catch (Exception ex)
        //{
        //    Console.WriteLine(ex.Message);
        //}

        //return games!;

        GameDto[] games = new GameDto[16];

        try
        {
            var client = _httpClientFactory.CreateClient("vortex");

            games = await client.GetFromJsonAsync<GameDto[]>(
                    $"Games/REG/5");
        }

        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return games!;
    }
}