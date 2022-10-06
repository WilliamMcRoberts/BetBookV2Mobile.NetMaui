using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.GlobalStateManagement;

public class AuthenticationState
{
    public AuthenticationStateModel CurrentAuthenticationState { get; set; } = new();

    public AuthenticationStateModel GetCurrentAuthenticationState() => CurrentAuthenticationState;
}
