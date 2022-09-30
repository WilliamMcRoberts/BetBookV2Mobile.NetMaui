

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetUserByIdQuery(string id) : IRequest<UserModel>;

