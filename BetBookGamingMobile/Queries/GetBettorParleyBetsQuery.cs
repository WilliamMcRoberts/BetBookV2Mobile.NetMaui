

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetBettorParleyBetsQuery(string userId) : IRequest<IEnumerable<ParleyBetSlipModel>>;

