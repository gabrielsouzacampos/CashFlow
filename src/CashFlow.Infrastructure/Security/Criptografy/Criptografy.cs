using CashFlow.Domain.Security.Criptografy;

namespace CashFlow.Infrastructure.Security.Criptografy;

public class Criptografy : IPasswordEncripter
{
    public string Encrypt(string password) => BCrypt.Net.BCrypt.HashPassword(password);
}

