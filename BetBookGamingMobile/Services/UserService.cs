


using BetBookGamingMobile.Models;
using Newtonsoft.Json;
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

            user = 
                await client.GetFromJsonAsync<UserModel>($"Users/UserId/{userId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return user;
    }

    public async Task<UserModel> GetUserByObjectId(string objectId)
    {
        UserModel user = new();
        try
        {
            var client = _httpClientFactory.CreateClient("vortex");

            user = 
                await client.GetFromJsonAsync<UserModel>($"Users/ObjectId/{objectId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return user;
    }

    public async Task<bool> CreateUser(UserModel user)
    {
        var client = _httpClientFactory.CreateClient("vortex");

        try
        {
            string json = JsonConvert.SerializeObject(user);

            var httpContent =
                new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            await client.PostAsync("Users", httpContent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }

    public async Task<bool> UpdateUser(UserModel user)
    {
        var client = _httpClientFactory.CreateClient("vortex");

        try
        {
            string json = JsonConvert.SerializeObject(user);

            var httpContent =
                new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            await client.PutAsync("Users", httpContent);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return false;
        }
    }
}
