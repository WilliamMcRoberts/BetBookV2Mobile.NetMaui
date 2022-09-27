
using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record DeleteBetCommand(CreateBetModel bet) : IRequest<BetSlipStateModel>;

