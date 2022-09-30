

using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserModel>
{
	private readonly IUserService _userService;

	public GetUserByIdHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<UserModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		return await _userService.GetUserByUserId(request.id);
	}
}
