

using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using MediatR;
using Microsoft.AspNetCore.Components.Authorization;
using AuthenticationState = BetBookGamingMobile.GlobalStateManagement.AuthenticationState;

namespace BetBookGamingMobile.Handlers;

public class GetCurrentAuthenticationStateHandler : IRequestHandler<GetCurrentAuthenticationStateQuery, AuthenticationStateModel>
{
	private readonly AuthenticationState _authenticationState;

	public GetCurrentAuthenticationStateHandler(AuthenticationState authenticationState)
	{
		_authenticationState = authenticationState;
	}

	public Task<AuthenticationStateModel> Handle(GetCurrentAuthenticationStateQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_authenticationState.GetCurrentAuthenticationState());
	}
}
