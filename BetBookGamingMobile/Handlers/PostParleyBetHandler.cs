

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class PostParleyBetHandler : IRequestHandler<PostParleyBetCommand, bool>
{
	private readonly IParleyBetSlipService _parleyBetSlipService;

	public PostParleyBetHandler(IParleyBetSlipService parleyBetSlipService)
	{
		_parleyBetSlipService = parleyBetSlipService;
	}

	public async Task<bool> Handle(PostParleyBetCommand request, CancellationToken cancellationToken)
	{
		return await _parleyBetSlipService.CreateParleyBet(request.parleyBetSlip);
	}
}
