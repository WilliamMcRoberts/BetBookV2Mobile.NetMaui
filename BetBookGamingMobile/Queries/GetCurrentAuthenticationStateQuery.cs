

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetCurrentAuthenticationStateQuery() : IRequest<AuthenticationStateModel>;

