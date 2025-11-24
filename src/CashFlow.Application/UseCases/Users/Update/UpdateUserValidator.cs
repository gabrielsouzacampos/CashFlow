using CashFlow.Communication.Requests;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCases.Users.Update;

public class UpdateUserValidator : AbstractValidator<RequestUpdateUserJson>
{
    public UpdateUserValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(
                user => string.IsNullOrWhiteSpace(user.Email).Equals(false),
                ApplyConditionTo.CurrentValidator
            )
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}
