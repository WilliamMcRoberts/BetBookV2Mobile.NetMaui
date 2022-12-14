

namespace BetBookGamingMobile.Models;

public class AuthenticationStateModel
{
    public UserModel LoggedInUser { get; set; }
    public string ObjectId { get; set; }
    public string DisplayName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string EmailAddress { get; set; }
    public string JobTitle { get; set; }
}
