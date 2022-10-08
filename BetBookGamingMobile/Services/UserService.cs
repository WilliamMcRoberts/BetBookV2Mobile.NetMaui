
namespace BetBookGamingMobile.Services;

public class UserService : BaseService, IUserService
{
    public UserService(IConnectivity connectivity) : base(connectivity)
    {
        SetBaseURL(Constants.VortexURL);
    }

    public async Task<UserModel> GetUserByUserId(string userId)
    {
        var user = new UserModel();
        try
        {
            var resourceUri = $"Users/UserId/{userId}";

            user = await GetAsync<UserModel>(resourceUri);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        
        return user;
    }

    public async Task<UserModel> GetUserByObjectId(string objectId)
    {
        var user = new UserModel();
        try
        {
            var resourceUri = $"Users/ObjectId/{objectId}";

            user = await GetAsync<UserModel>(resourceUri);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        return user;
    }

    public async Task<bool> CreateUser(UserModel user)
    {
        await PostAsync<UserModel>("Users", user);

        return true;
    }

    public async Task<bool> UpdateUser(UserModel user)
    {
        await PutAsync<UserModel>("Users", user);

        return true;
    }
}
