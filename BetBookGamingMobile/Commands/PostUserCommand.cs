

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record PostUserCommand(UserModel user) : IRequest<bool>;

