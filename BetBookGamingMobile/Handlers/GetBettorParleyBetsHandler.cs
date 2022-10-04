

using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetBettorParleyBetsHandler : IRequestHandler<GetBettorParleyBetsQuery, List<ParleyBetSlipModel>>
{
	private readonly IParleyBetSlipService _parleyBetSlipService;

	public GetBettorParleyBetsHandler(IParleyBetSlipService parleyBetSlipService)
	{
		_parleyBetSlipService = parleyBetSlipService;
	}

	public async Task<List<ParleyBetSlipModel>> Handle(GetBettorParleyBetsQuery request, CancellationToken cancellationToken)
	{
		return await _parleyBetSlipService.GetAllBettorParleyBets(request.userId);
	}
}
