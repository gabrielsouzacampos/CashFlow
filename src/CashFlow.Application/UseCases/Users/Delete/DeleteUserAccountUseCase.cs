using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCases.Users.Delete;

public class DeleteUserAccountUseCase(ILoggedUser loggedUser, IUnitOfWork unitOfWork, IUsersRepository repository) : IDeleteUserAccountUseCase
{
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IUsersRepository _repository = repository;

    public async Task Execute()
    {
        var user = await _loggedUser.Get();
     
        _repository.Delete(user);
     
        await _unitOfWork.Commit();
    }
}
