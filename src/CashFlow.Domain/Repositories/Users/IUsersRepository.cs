using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;

public interface IUsersRepository
{
    Task<bool> ExistActiveUserWithEmail(string email);

    Task Add(User user);
}

