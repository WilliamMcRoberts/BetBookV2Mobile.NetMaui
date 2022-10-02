

using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetUserByObjectIdHandler : IRequestHandler<GetUserByObjectIdQuery, UserModel>
{
	private readonly IUserService _userService;

	public GetUserByObjectIdHandler(IUserService userService)
	{
		_userService = userService;
	}

	public async Task<UserModel> Handle(GetUserByObjectIdQuery request, CancellationToken cancellationToken)
	{
		return await _userService.GetUserByObjectId(request.objectId);
	}
}
