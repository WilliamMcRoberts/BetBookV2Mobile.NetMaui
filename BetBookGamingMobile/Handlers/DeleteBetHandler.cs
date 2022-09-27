

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class DeleteBetHandler : IRequestHandler<DeleteBetCommand, BetSlipStateModel>
{
    private readonly BetSlipState _betSlip;

    public DeleteBetHandler(BetSlipState betSlip)
    {
        _betSlip = betSlip;
    }

    public Task<BetSlipStateModel> Handle(
        DeleteBetCommand request, CancellationToken cancellationToken)
    {
        _betSlip.RemoveBetFromPreBetsList(request.bet);
        return Task.FromResult(_betSlip.GetBetSlipState());
    }
}
