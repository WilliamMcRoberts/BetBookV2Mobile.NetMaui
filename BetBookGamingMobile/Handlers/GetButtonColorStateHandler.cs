

using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetButtonColorStateHandler : IRequestHandler<GetButtonColorStateQuery, ButtonColorStateModel>
{
	private readonly BetSlip _betSlip;

	public GetButtonColorStateHandler(BetSlip betSlip)
	{
		_betSlip = betSlip;
	}

	public async Task<ButtonColorStateModel> Handle(GetButtonColorStateQuery request, CancellationToken cancellationToken)
	{
		return await Task.FromResult(_betSlip.GetButtonColorState(request.gameDto));
	}
}
