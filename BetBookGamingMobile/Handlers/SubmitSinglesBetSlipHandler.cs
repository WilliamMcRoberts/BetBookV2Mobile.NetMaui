using BetBookGamingMobile.Commands;
using BetBookGamingMobile.StateManagement;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetBookGamingMobile.Handlers;

public class SubmitSinglesBetSlipHandler : IRequestHandler<SubmitSinglesBetSlipCommand, BetSlipState>
{
	private readonly BetSlip _betSlip;

	public SubmitSinglesBetSlipHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public async Task<BetSlipState> Handle(
		SubmitSinglesBetSlipCommand request, CancellationToken cancellationToken)
	{
		await _betSlip.OnSubmitBetsFromSinglesBetSlip(request.loggedInUser);
		return await Task.FromResult(_betSlip.GetBetSlipState());
	}
}
