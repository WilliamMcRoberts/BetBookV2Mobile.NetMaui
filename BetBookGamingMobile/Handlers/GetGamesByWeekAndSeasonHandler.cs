

using BetBookGamingMobile.Dto;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using BetBookGamingMobile.Services;
using MediatR;

namespace BetBookGamingMobile.Handlers;

public class GetGamesByWeekAndSeasonHandler : IRequestHandler<GetGamesByWeekAndSeasonQuery, GameDto[]>
{
	private readonly IGameService _gameService;

	public GetGamesByWeekAndSeasonHandler(IGameService gameService)
	{
		_gameService = gameService;
	}

	public async Task<GameDto[]> Handle(
		GetGamesByWeekAndSeasonQuery request, CancellationToken cancellationToken)
	{
		return await _gameService.GetGamesByWeekAndSeason(request.week, request.season);
	}
}
