using AutoMapper;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;

namespace CashFlow.Application.UseCases.Expenses.Register;

public class RegisterExpenseUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser) : IRegisterExpenseUseCase
{
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ILoggedUser _loggedUser = loggedUser;
    private readonly IExpensesRepository _repository = repository;

    public async Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request)
    {
        Validate(request);

        var loggedUser = await _loggedUser.Get();

        var expense = _mapper.Map<Expense>(request);
        expense.UserId = loggedUser.Id;

        await _repository.Add(expense);
        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisterExpenseJson>(expense);
    }


    private static void Validate(RequestExpenseJson request)
    {
        var result = new ExpenseValidator().Validate(request);

        if (!result.IsValid)
        {
            var errorMessages = result.Errors.Select(f => f.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}

