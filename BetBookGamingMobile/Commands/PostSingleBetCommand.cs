

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record PostSingleBetCommand(SingleBetModel singleBet) : IRequest<bool>;

