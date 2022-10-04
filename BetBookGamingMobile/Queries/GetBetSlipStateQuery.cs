

using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetBetSlipStateQuery() : IRequest<BetSlipStateModel>;

