using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.State;

public class AuthenticationState
{
    public AuthenticationStateModel CurrentAuthenticationState { get; } = new();

    public AuthenticationStateModel GetCurrentAuthenticationState() => CurrentAuthenticationState;
}
