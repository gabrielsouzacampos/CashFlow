using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestUtilities.Repositories;

public class UsersRepositoryBuilder
{
    private readonly Mock<IUsersRepository> _repository;

    public UsersRepositoryBuilder()
    {
        _repository = new Mock<IUsersRepository>();
    }

    public void ExistActiveUserWithEmail(string email) 
        => _repository.Setup(userRepository => userRepository.ExistActiveUserWithEmail(email)).ReturnsAsync(true);

    public UsersRepositoryBuilder GetUserByEmail(User user)
    {
        _repository.Setup(userRepository => userRepository.GetUserByEmail(user.Email)).ReturnsAsync(user);

        return this;
    }

    public UsersRepositoryBuilder GetById(User user)
    {
        _repository.Setup(repository => repository.GetById(user.Id)).ReturnsAsync(user);
        
        return this;
    }

    public IUsersRepository Build() => _repository.Object;
}

