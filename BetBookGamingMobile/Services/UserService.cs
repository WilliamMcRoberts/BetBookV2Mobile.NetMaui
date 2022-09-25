


using BetBookGamingMobile.Models;
using System.Net.Http;
using System.Net.Http.Json;

namespace BetBookGamingMobile.Services;

public class UserService : IUserService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserService(IHttpClientFactory httpClientFactory)
    {

        _httpClientFactory = httpClientFactory;
    }

    public async Task<UserModel> GetUserByUserId(string userId)
    {
        UserModel user = new();
        try
        {
            var client = _httpClientFactory.CreateClient("vortex");

            user = await client.GetFromJsonAsync<UserModel>($"Users/UserId/{userId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message); ;
        }
        return user;
    }
}
