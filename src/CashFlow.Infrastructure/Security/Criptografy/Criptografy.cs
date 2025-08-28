using CashFlow.Domain.Security.Criptografy;

namespace CashFlow.Infrastructure.Security.Criptografy;

public class Criptografy : IPasswordEncripter
{
    public string Encrypt(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    public bool Verify(string password, string passwordHash) => BCrypt.Net.BCrypt.Verify(password, passwordHash);
}

