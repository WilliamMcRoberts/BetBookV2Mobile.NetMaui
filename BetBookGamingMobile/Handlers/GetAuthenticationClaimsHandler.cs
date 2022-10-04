

using BetBookGamingMobile.Auth;
using BetBookGamingMobile.Models;
using BetBookGamingMobile.Queries;
using MediatR;
using System.IdentityModel.Tokens.Jwt;

namespace BetBookGamingMobile.Handlers;

public class GetAuthenticationClaimsHandler : IRequestHandler<GetAuthenticationClaimsQuery, JwtSecurityToken>
{
	private readonly IAuthService _authService;

	public GetAuthenticationClaimsHandler(IAuthService authService)
	{
		_authService = authService;
	}

	public async Task<JwtSecurityToken> Handle(GetAuthenticationClaimsQuery request, CancellationToken cancellationToken)
	{
		return await _authService.GetAuthClaims();
	}
}
