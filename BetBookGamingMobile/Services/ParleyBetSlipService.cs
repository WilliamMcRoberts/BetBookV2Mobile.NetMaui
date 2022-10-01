

using BetBookGamingMobile.Models;
using Newtonsoft.Json;

namespace BetBookGamingMobile.Services;

public class ParleyBetSlipService : IParleyBetSlipService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ParleyBetSlipService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<bool> CreateParleyBet(ParleyBetSlipModel parleyBet)
    {
        var client = _httpClientFactory.CreateClient("vortex");

        try
        {
            string json = JsonConvert.SerializeObject(parleyBet);

            var httpContent =
                new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            await client.PostAsync("ParleyBetSlips", httpContent);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
