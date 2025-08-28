namespace CashFlow.Domain.Security.Criptografy;

public interface IPasswordEncripter
{
    string Encrypt(string password);
}
