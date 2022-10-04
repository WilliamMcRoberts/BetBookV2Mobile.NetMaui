

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetUserByUserIdQuery(string id) : IRequest<UserModel>;

