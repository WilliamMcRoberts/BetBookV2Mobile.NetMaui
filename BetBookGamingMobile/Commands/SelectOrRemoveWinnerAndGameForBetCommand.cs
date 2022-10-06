

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.GlobalStateManagement;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record SelectOrRemoveWinnerAndGameForBetCommand(
    string winner, GameDto gameDto, BetType betType) : IRequest<(BetSlipStateModel, ButtonColorStateModel)>;