using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.Services
{
    public interface IUserService
    {
        Task<bool> CreateUser(UserModel user);
        Task<UserModel> GetUserByObjectId(string objectId);
        Task<UserModel> GetUserByUserId(string userId);
        Task<bool> UpdateUser(UserModel user);
    }
}