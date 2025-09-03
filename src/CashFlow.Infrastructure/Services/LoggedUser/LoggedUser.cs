using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Domain.Tokens;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infrastructure.Services.LoggedUser;

public class LoggedUser(CashFlowDbContext context, ITokenProvider tokenProvider) : ILoggedUser
{
    private readonly CashFlowDbContext _context = context;
    private readonly ITokenProvider _tokenProvider = tokenProvider;

    public async Task<User> Get()
    {
        var token = _tokenProvider.TokenOnRequest();
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = tokenHandler.ReadJwtToken(token);
        var identifier = jwtSecurityToken.Claims.First(claim => claim.Type.Equals(ClaimTypes.Sid)).Value;

        return await _context.Users.AsNoTracking()
            .FirstAsync(user => user.UserIdentifier.Equals(Guid.Parse(identifier)));
    }
}
