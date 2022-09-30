

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class SubmitParleyBetSlipHandler : IRequestHandler<SubmitParleyBetSlipCommand, BetSlipState>
{
	private readonly BetSlip _betSlip;

	public SubmitParleyBetSlipHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public async Task<BetSlipState> Handle(SubmitParleyBetSlipCommand request, CancellationToken cancellationToken)
	{
		await _betSlip.OnSubmitBetsFromParleyBetSlip(request.loggedInUser, request.parleyWagerAmount);
		return await Task.FromResult(_betSlip.GetBetSlipState());
	}
}
