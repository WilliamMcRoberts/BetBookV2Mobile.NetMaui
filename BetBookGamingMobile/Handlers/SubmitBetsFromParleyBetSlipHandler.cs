

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.GlobalStateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class SubmitBetsFromParleyBetSlipHandler : IRequestHandler<SubmitBetsFromParleyBetSlipCommand, bool>
{
	private readonly BetSlip _betSlip;

	public SubmitBetsFromParleyBetSlipHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public async Task<bool> Handle(SubmitBetsFromParleyBetSlipCommand request, CancellationToken cancellationToken)
	{
		return await _betSlip.OnSubmitBetsFromParleyBetSlip(request.loggedInUser, request.parleyWagerAmount);
	}
}
