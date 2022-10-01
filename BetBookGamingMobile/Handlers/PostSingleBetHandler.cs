

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Services;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class PostSingleBetHandler : IRequestHandler<PostSingleBetCommand, bool>
{
	private readonly ISingleBetService _singleBetService;

	public PostSingleBetHandler(ISingleBetService singleBetService)
	{
		_singleBetService = singleBetService;
	}

	public async Task<bool> Handle(PostSingleBetCommand request, CancellationToken cancellationToken)
	{
		return await _singleBetService.CreateSingleBet(request.singleBet);
	}
}
