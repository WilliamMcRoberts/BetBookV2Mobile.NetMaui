

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record PostParleyBetCommand(ParleyBetSlipModel parleyBetSlip) : IRequest<bool>;
