using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetBetSlipStateHandler : IRequestHandler<GetBetSlipStateQuery, BetSlipState>
{
	private readonly BetSlip _betSlip;

	public GetBetSlipStateHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public Task<BetSlipState> Handle(
		GetBetSlipStateQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_betSlip.GetBetSlipState());
	}
}
