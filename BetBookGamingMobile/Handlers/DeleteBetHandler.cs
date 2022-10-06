

using BetBookGamingMobile.Commands;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.GlobalStateManagement;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class DeleteBetHandler : IRequestHandler<DeleteBetCommand, BetSlipStateModel>
{
    private readonly BetSlip _betSlip;

    public DeleteBetHandler(BetSlip betSlip)
    {
        _betSlip = betSlip;
    }

    public async Task<BetSlipStateModel> Handle(
        DeleteBetCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(_betSlip.RemoveBetFromPreBetsList(request.bet));
    }
}
