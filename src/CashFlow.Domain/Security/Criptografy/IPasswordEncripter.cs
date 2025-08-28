namespace CashFlow.Domain.Security.Criptografy;

public interface IPasswordEncripter
{
    string Encrypt(string password);

    bool Verify(string password, string passwordHash);
}
