

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class PostUserHandler : IRequestHandler<PostUserCommand, bool>
{
	private readonly IUserService _userService;

	public PostUserHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<bool> Handle(PostUserCommand request, CancellationToken cancellationToken)
	{
		return await _userService.CreateUser(request.user);
	}
}
