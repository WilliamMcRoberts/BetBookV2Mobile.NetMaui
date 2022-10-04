

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetStateForGameDetailsPageHandler : IRequestHandler<GetStateForGameDetailsPageQuery, (BetSlipStateModel, ButtonColorStateModel, ButtonTextStateModel)>
{
	private readonly BetSlip _betSlip;

	public GetStateForGameDetailsPageHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public Task<(BetSlipStateModel, ButtonColorStateModel, ButtonTextStateModel)> Handle(
        GetStateForGameDetailsPageQuery request, CancellationToken cancellationToken)
	{
        return Task.FromResult(
			(_betSlip.GetAllStates(request.gameDto)));
    }
}
