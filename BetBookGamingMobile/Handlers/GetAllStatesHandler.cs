

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetAllStatesHandler : IRequestHandler<GetAllStatesQuery, (BetSlipState, ButtonColorState, ButtonTextState)>
{
	private readonly BetSlip _betSlip;

	public GetAllStatesHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public Task<(BetSlipState, ButtonColorState, ButtonTextState)> Handle(
        GetAllStatesQuery request, CancellationToken cancellationToken)
	{
        return Task.FromResult(
			(_betSlip.GetAllStates(request.gameDto)));
    }
}
