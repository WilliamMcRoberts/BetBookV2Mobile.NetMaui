

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class PutUserHandler : IRequestHandler<PutUserCommand, bool>
{
	private readonly IUserService _userService;

	public PutUserHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<bool> Handle(PutUserCommand request, CancellationToken cancellationToken)
	{
		return await _userService.UpdateUser(request.user);
	}
}
