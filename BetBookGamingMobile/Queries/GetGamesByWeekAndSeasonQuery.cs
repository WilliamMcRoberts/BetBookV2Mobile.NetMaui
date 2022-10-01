
using BetBookGamingMobile.Dto;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetGamesByWeekAndSeasonQuery(int week, SeasonType season) : IRequest<GameDto[]>;

