using AutoMapper;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;

namespace CashFlow.Application.UseCases.Expenses.GetById;

public class GetExpenseByIdUseCase(IExpensesRepository repository, IMapper mapper, ILoggedUser loggedUser) : IGetExpenseByIdUseCase
{
    private readonly IMapper _mapper = mapper;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IExpensesRepository _repository = repository;

    public async Task<ResponseExpenseJson> Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();
        var result = await _repository.GetById(id, loggedUser);

        return result is null
            ? throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND)
            : _mapper.Map<ResponseExpenseJson>(result);
    }
}

