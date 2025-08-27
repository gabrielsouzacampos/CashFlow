using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;

namespace CashFlow.Application.UseCases.Expenses.Delete;

public class DeleteExpenseUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork) : IDeleteExpenseUseCase
{
    private readonly IExpensesRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Execute(long id)
    {
        var result = await _repository.DeleteById(id);

        if (!result)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);


        await _unitOfWork.Commit();
    }
}

