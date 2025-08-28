using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Security.Tokens;

public interface IAccessTokenGenerator
{
    string GenerateAccessToken(User user);
}
