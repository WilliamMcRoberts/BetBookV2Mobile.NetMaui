

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record SubmitBetsFromParleyBetSlipCommand(UserModel loggedInUser, decimal parleyWagerAmount) : IRequest<bool>;

