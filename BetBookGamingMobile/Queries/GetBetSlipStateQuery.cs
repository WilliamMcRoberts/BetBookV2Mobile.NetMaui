

using BetBookGamingMobile.Models;
using BetBookGamingMobile.GlobalStateManagement;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetBetSlipStateQuery() : IRequest<BetSlipStateModel>;

