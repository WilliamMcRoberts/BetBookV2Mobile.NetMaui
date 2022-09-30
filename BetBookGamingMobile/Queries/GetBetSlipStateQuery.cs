

using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetBetSlipStateQuery() : IRequest<BetSlipState>;

