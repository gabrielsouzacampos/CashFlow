using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Exception;
using CommomTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.Users.Update;

public class UpdateUserValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ErrorNameEmpty(string name)
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            () => result.Errors.ShouldHaveSingleItem(),
            () => result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY))
        );
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void ErrorEmailEmpty(string email)
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            () => result.Errors.ShouldHaveSingleItem(),
            () => result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY))
        );
    }

    [Fact]
    public void ErrorEmailInvalid()
    {
        var validator = new UpdateUserValidator();
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Email = "gabriel.test";

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            () => result.Errors.ShouldHaveSingleItem(),
            () => result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID))
        );
    }
}
