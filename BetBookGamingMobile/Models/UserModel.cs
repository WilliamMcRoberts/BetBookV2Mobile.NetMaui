

namespace BetBookGamingMobile.Models;


public class UserModel
{
    public string UserId { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string EmailAddress { get; set; }

    public string ObjectIdentifier { get; set; }

    public string DisplayName { get; set; }

    public decimal AccountBalance { get; set; }

    public string BalanceDisplay { get => $"{AccountBalance:C}"; }
}
