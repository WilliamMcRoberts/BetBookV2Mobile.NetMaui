
using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Commands;


public record SetCurrentAuthenticationStateCommand(AuthenticationStateModel currentAuthenticationState) : IRequest<AuthenticationStateModel>;

