using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.State;

public class AuthenticationState
{
    public AuthenticationStateModel CurrentAuthenticationState { get; set; } = new();

    public AuthenticationStateModel GetCurrentAuthenticationState() => CurrentAuthenticationState;
}
