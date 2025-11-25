using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Users.ChangePassword;

public class ChangePasswordUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task ErrorNewPasswordEmpty()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        request.NewPassword = string.Empty;

        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();

        var errors = result.GetErrors();

        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.INVALID_PASSWORD)
        );
    }

    [Fact]
    public async Task ErrorCurrentPasswordDifferent()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        var useCase = CreateUseCase(user);
        
        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        
        var errors = result.GetErrors();

        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD)
        );
    }

    private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = new PasswordEncrypterBuilder().Verify(password).Build();

        var repository = new UsersRepositoryBuilder();

        repository.GetById(user);

        return new ChangePasswordUseCase(loggedUser, unitOfWork, repository.Build(), passwordEncripter);
    }
}

