using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashFlow.Infrastructure.Security.Tokens;

public class JwtTokenGenerator(uint expirationTimeMinutes, string signingKey) : IAccessTokenGenerator
{
    private readonly uint _expirationTimeMinutes = expirationTimeMinutes;
    private readonly string _signingKey = signingKey;

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim> 
        {
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Sid, user.UserIdentifier.ToString()),
            new(ClaimTypes.Role, user.Role)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(_expirationTimeMinutes),
            Subject = new ClaimsIdentity(claims),
            SigningCredentials =
                new SigningCredentials(SecurityKey(), SecurityAlgorithms.HmacSha256Signature),
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
    }

    private SymmetricSecurityKey SecurityKey()
        => new(Encoding.UTF8.GetBytes(_signingKey));
}
