

using BetBookGamingMobile.Models;
using Newtonsoft.Json;

namespace BetBookGamingMobile.Services;

public class SingleBetService : ISingleBetService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public SingleBetService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> CreateSingleBet(SingleBetModel singleBet)
    {
        var client = _httpClientFactory.CreateClient("vortex");

        try
        {
            string json = JsonConvert.SerializeObject(singleBet);

            var httpContent =
                new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            await client.PostAsync("SingleBets", httpContent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
