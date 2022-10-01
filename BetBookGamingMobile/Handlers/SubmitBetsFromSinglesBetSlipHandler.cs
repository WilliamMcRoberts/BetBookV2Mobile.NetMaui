

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class SubmitBetsFromSinglesBetSlipHandler : IRequestHandler<SubmitBetsFromSinglesBetSlipCommand, bool>
{
	private readonly BetSlip _betSlip;

	public SubmitBetsFromSinglesBetSlipHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public async Task<bool> Handle(SubmitBetsFromSinglesBetSlipCommand request, CancellationToken cancellationToken)
	{
		return await _betSlip.OnSubmitBetsFromSinglesBetSlip(request.loggedInUser);
	}
}
