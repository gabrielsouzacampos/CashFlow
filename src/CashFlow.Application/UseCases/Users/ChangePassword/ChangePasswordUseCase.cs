using CashFlow.Communication.Requests;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Security.Criptografy;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using FluentValidation.Results;

namespace CashFlow.Application.UseCases.Users.ChangePassword;

public class ChangePasswordUseCase(
    ILoggedUser loggedUser, IUnitOfWork unitOfWork,
    IUsersRepository repository, IPasswordEncripter passwordEncripter) : IChangePasswordUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUsersRepository _repository = repository;
    private readonly IPasswordEncripter _passwordEncripter = passwordEncripter;

    public async Task Execute(RequestChangePasswordJson request)
    {
        var loggedUser = await _loggedUser.Get();

        Validate(request, loggedUser);

        var user = await _repository.GetById(loggedUser.Id);
        user!.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _repository.Update(user);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, User loggedUser)
    {
        var validator = new ChangePasswordValidator();
        var result = validator.Validate(request);

        var passwordHash = _passwordEncripter.Verify(request.Password, loggedUser.Password);

        if (!passwordHash)
            result.Errors.Add(new ValidationFailure(
                string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD)
            );

        if (!result.IsValid)
        {
            var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidationException(errors);
        }
    }
}
