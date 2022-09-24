
using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.Services
{
    public interface IUserService
    {
        Task<UserModel> GetUserByUserId(string userId = "632395fdc17912bd030e4162");
    }
}