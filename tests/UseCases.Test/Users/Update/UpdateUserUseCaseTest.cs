using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Users.Update;

public class UpdateUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.ShouldNotThrowAsync();

        user.Name.ShouldBe(request.Name);
        user.Email.ShouldBe(request.Email);
    }

    [Fact]
    public async Task ErrorNameEmpty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        var errors = result.GetErrors();

        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.NAME_EMPTY)
        );
    }

    [Fact]
    public async Task ErrorEmailEmpty()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = string.Empty;
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        var errors = result.GetErrors();

        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.EMAIL_EMPTY)
        );
    }

    [Fact]
    public async Task ErrorEmailInvalid()
    {
        var user = UserBuilder.Build();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "gabriel.test";
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        var errors = result.GetErrors();

        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.EMAIL_INVALID)
        );
    }

    private UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var repository = new UsersRepositoryBuilder();

        repository.GetById(user);

        if (!string.IsNullOrWhiteSpace(email))
            repository.ExistActiveUserWithEmail(email);

        return new UpdateUserUseCase(LoggedUserBuilder.Build(user), repository.Build(), UnitOfWorkBuilder.Build());
    }
}

