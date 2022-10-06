using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.GlobalStateManagement;
using MediatR;

namespace BetBookGamingMobile.Queries;


public record GetStateForGameDetailsPageQuery(GameDto gameDto) : IRequest<(BetSlipStateModel, ButtonColorStateModel, ButtonTextStateModel)>;
