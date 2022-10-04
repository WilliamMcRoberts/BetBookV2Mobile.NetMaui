

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetBettorSingleBetsHandler : IRequestHandler<GetBettorSingleBetsQuery, List<SingleBetModel>>
{
	private readonly ISingleBetService _singleBetService;

	public GetBettorSingleBetsHandler(ISingleBetService singleBetService)
	{
		_singleBetService = singleBetService;
	}

	public async Task<List<SingleBetModel>> Handle(GetBettorSingleBetsQuery request, CancellationToken cancellationToken)
	{
		return await _singleBetService.GetAllBettorSingleBets(request.userId);
	}
}
