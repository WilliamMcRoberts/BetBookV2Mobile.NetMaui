

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class DeleteBetHandler : IRequestHandler<DeleteBetCommand, BetSlipState>
{
    private readonly BetSlip _betSlip;

    public DeleteBetHandler(BetSlip betSlip)
    {
        _betSlip = betSlip;
    }

    public Task<BetSlipState> Handle(
        DeleteBetCommand request, CancellationToken cancellationToken)
    {
        _betSlip.RemoveBetFromPreBetsList(request.bet);
        return Task.FromResult(_betSlip.GetBetSlipState());
    }
}
