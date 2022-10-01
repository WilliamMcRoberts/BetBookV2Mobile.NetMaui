using BetBookGamingMobile.Models;
using MediatR;


namespace BetBookGamingMobile.Commands;

public record SubmitBetsFromSinglesBetSlipCommand(UserModel loggedInUser) : IRequest<bool>;
