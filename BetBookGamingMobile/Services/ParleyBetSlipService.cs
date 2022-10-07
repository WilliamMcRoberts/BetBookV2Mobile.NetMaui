

using BetBookGamingMobile.Models;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace BetBookGamingMobile.Services;

public class ParleyBetSlipService : IParleyBetSlipService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ParleyBetSlipService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ParleyBetSlipModel>> GetAllBettorParleyBets(string userId)
    {
        List<ParleyBetSlipModel> bettorParleyBets = new();
        try
        {
            var client = _httpClientFactory.CreateClient("vortex");

            bettorParleyBets =
                await client.GetFromJsonAsync<List<ParleyBetSlipModel>>(
                    $"ParleyBetSlips/BettorId={userId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return bettorParleyBets;
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
