

using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetBettorParleyBetsHandler : IRequestHandler<GetBettorParleyBetsQuery, IEnumerable<ParleyBetSlipModel>>
{
	private readonly IParleyBetSlipService _parleyBetSlipService;

	public GetBettorParleyBetsHandler(IParleyBetSlipService parleyBetSlipService)
	{
		_parleyBetSlipService = parleyBetSlipService;
	}

	public async Task<IEnumerable<ParleyBetSlipModel>> Handle(GetBettorParleyBetsQuery request, CancellationToken cancellationToken)
	{
		return await _parleyBetSlipService.GetAllBettorParleyBets(request.userId);
	}
}
