

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.GlobalStateManagement;
using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class SetCurrentAuthenticationStateHandler : IRequestHandler<SetCurrentAuthenticationStateCommand, AuthenticationStateModel>
{
	private readonly AuthenticationState _authState;

	public SetCurrentAuthenticationStateHandler(AuthenticationState authState)
	{
		_authState = authState;
	}

	public Task<AuthenticationStateModel> Handle(SetCurrentAuthenticationStateCommand request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_authState.CurrentAuthenticationState = request.currentAuthenticationState);
	}
}
