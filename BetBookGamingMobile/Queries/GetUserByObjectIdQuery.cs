

using BetBookGamingMobile.Models;
using MediatR;

namespace BetBookGamingMobile.Queries;

public record GetUserByObjectIdQuery(string objectId) : IRequest<UserModel>;

