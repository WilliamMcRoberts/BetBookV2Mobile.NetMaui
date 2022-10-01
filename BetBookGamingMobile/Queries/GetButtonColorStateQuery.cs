using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using MediatR;


namespace BetBookGamingMobile.Queries;

public record GetButtonColorStateQuery(GameDto gameDto) : IRequest<ButtonColorState>;

