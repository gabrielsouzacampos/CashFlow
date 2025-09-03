using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork, ILoggedUser loggedUser) : IDeleteExpenseUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILoggedUser _loggedUser = loggedUser;

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        
        _ = await _repository.GetExpenseById(id, loggedUser)
            ?? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _repository.DeleteById(id);

        await _unitOfWork.Commit();
    }
}

