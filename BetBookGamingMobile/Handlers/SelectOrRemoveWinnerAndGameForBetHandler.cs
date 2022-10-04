

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class SelectOrRemoveWinnerAndGameForBetHandler : IRequestHandler<SelectOrRemoveWinnerAndGameForBetCommand, (BetSlipStateModel, ButtonColorStateModel)>
{
	private readonly BetSlip _betSlip;

	public SelectOrRemoveWinnerAndGameForBetHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public async Task<(BetSlipStateModel, ButtonColorStateModel)> Handle(SelectOrRemoveWinnerAndGameForBetCommand request, CancellationToken cancellationToken)
	{
		return await Task.FromResult(_betSlip.SelectOrRemoveWinnerAndGameForBet(request.winner, request.gameDto, request.betType));
		
	}
}
