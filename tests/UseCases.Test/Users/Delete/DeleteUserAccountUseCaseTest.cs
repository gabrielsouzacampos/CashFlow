using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Users.Delete;

public class DeleteUserAccountUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.ShouldNotThrowAsync();
    }

    private DeleteUserAccountUseCase CreateUseCase(User user)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var repository = new UsersRepositoryBuilder();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteUserAccountUseCase(loggedUser, unitOfWork, repository.Build());
    }
}

