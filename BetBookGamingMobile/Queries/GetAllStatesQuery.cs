using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Queries;


public record GetAllStatesQuery(GameDto gameDto) : IRequest<(BetSlipState,ButtonColorState,ButtonTextState)>;
