
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record DeleteBetCommand(CreateBetModel bet) : IRequest<BetSlipState>;

