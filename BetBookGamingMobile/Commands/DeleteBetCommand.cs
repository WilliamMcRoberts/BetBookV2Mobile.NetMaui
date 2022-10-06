
using BetBookGamingMobile.Models;
using BetBookGamingMobile.GlobalStateManagement;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record DeleteBetCommand(CreateBetModel bet) : IRequest<BetSlipStateModel>;

