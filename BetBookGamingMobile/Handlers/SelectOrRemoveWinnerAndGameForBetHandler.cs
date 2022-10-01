

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class SelectOrRemoveWinnerAndGameForBetHandler : IRequestHandler<SelectOrRemoveWinnerAndGameForBetCommand, BetSlipState>
{
	private readonly BetSlip _betSlip;

	public SelectOrRemoveWinnerAndGameForBetHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public async Task<BetSlipState> Handle(SelectOrRemoveWinnerAndGameForBetCommand request, CancellationToken cancellationToken)
	{
		_betSlip.SelectOrRemoveWinnerAndGameForBet(request.winner, request.gameDto, request.betType);
		return await Task.FromResult((_betSlip.GetBetSlipState()));
	}
}
