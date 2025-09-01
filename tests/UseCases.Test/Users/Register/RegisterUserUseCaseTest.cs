using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using CommomTestUtilities.Token;
using Shouldly;

namespace UseCases.Test.Users.Register;

public class RegisterUserUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Build();

        var result = await useCase.Execute(request);

        result.ShouldNotBeNull();
        result.Name.ShouldBeEquivalentTo(request.Name);
        result.Token.ShouldNotBeNullOrWhiteSpace();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Error_Name_Empty(string name)
    {
        var useCase = CreateUseCase();
        var request = RequestRegisterUserJsonBuilder.Build();
        request.Name = name;

        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        var errors = result.GetErrors();

        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.NAME_EMPTY)
        );
    }

    [Fact]
    public async Task Error_Email_Already_Exist()
    {
        var request = RequestRegisterUserJsonBuilder.Build();
        var useCase = CreateUseCase(request.Email);

        var act = async () => await useCase.Execute(request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        var errors = result.GetErrors();

        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.EMAIL_ALREADY_REGISTERED)
        );
    }

    private static RegisterUserUseCase CreateUseCase(string? email = null)
    {
        var repository = new UsersRepositoryBuilder();

        if (!string.IsNullOrWhiteSpace(email)) 
            repository.ExistActiveUserWithEmail(email);

        return new(
            MapperBuilder.Build(),
            new PasswordEncrypterBuilder().Build(),
            repository.Build(),
            UnitOfWorkBuilder.Build(),
            JwtTokenGeneratorBuilder.Build()
        );
    }
}
