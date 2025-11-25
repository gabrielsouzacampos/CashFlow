using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Exception;
using CommomTestUtilities.Requests;
using Shouldly;

namespace Validators.Test.Users.ChangePassword;

public class ChangePasswordValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();
        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }

    [Theory]
    [InlineData("")]
    [InlineData("  ")]
    [InlineData(null)]
    public void ErrorNewPasswordEmpty(string newPassword)
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = newPassword;

        var result = validator.Validate(request);

        result.IsValid.ShouldBeFalse();
        result.Errors.ShouldSatisfyAllConditions(
            () => result.Errors.ShouldHaveSingleItem(),
            () => result.Errors.ShouldContain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD))
        );
    }
}
