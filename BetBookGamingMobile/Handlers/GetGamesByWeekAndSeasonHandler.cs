

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetGamesByWeekAndSeasonHandler : IRequestHandler<GetGamesByWeekAndSeasonQuery, IEnumerable<GameDto>>
{
	private readonly IGameService _gameService;

	public GetGamesByWeekAndSeasonHandler(IGameService gameService)
	{
		_gameService = gameService;
	}

	public async Task<IEnumerable<GameDto>> Handle(
		GetGamesByWeekAndSeasonQuery request, CancellationToken cancellationToken)
	{
		return await _gameService.GetGamesByWeekAndSeason(request.week, request.season);
	}
}
