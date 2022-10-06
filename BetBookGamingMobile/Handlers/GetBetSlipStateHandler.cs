using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.GlobalStateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetBetSlipStateHandler : IRequestHandler<GetBetSlipStateQuery, BetSlipStateModel>
{
	private readonly BetSlip _betSlip;

	public GetBetSlipStateHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public Task<BetSlipStateModel> Handle(
		GetBetSlipStateQuery request, CancellationToken cancellationToken)
	{
		return Task.FromResult(_betSlip.GetBetSlipState());
	}
}
