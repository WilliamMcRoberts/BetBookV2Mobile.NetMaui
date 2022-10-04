

using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile.Queries;

public record GetAuthenticationClaimsQuery() : IRequest<JwtSecurityToken>;


