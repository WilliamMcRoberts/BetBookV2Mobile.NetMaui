

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record SelectOrRemoveWinnerAndGameForBetCommand(
    string winner, GameDto gameDto, BetType betType) : IRequest<BetSlipState>;