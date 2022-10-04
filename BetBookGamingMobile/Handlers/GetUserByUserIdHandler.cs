

using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetUserByUserIdHandler : IRequestHandler<GetUserByUserIdQuery, UserModel>
{
	private readonly IUserService _userService;

	public GetUserByUserIdHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<UserModel> Handle(GetUserByUserIdQuery request, CancellationToken cancellationToken)
	{
		return await _userService.GetUserByUserId(request.id);
	}
}
