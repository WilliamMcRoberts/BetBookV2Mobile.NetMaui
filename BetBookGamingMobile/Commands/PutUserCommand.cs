

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Commands;

public record PutUserCommand(UserModel user) : IRequest<bool>;

