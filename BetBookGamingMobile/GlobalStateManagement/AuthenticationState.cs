using BetBookGamingMobile.Models;

namespace BetBookGamingMobile.StateManagement;

public class AuthenticationState
{
    public AuthenticationStateModel CurrentAuthenticationState { get; set; } = new();

    public AuthenticationStateModel GetCurrentAuthenticationState() => CurrentAuthenticationState;
}
