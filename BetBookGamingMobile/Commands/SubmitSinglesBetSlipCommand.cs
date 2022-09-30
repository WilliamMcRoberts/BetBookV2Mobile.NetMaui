

using BetBookGamingMobile.Models;
using BetBookGamingMobile.StateManagement;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record SubmitSinglesBetSlipCommand(UserModel loggedInUser) : IRequest<BetSlipState>;

