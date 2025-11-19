using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Requests;
using Shouldly;

namespace UseCases.Test.Expenses.Update;

public class UpdateExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        var request = RequestExpenseJsonBuilder.Build();

        var useCase = CreateUseCase(loggedUser, expense);
        var act = async () => await useCase.Execute(expense.Id, request);

        await act.ShouldNotThrowAsync();

        expense.Title.ShouldBe(request.Title);
        expense.Description.ShouldBe(request.Description);
        expense.Date.ShouldBe(request.Date);
        expense.Amount.ShouldBe(request.Amount);
    }

    [Fact]
    public async Task ErrorTitleEmpty()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);
        var request = RequestExpenseJsonBuilder.Build();
        
        var useCase = CreateUseCase(loggedUser, expense);
        request.Title = string.Empty;

        var act = async () => await useCase.Execute(expense.Id, request);
        var result = await act.ShouldThrowAsync<ErrorOnValidationException>();
        var errors = result.GetErrors();

        result.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.TITLE_REQUIRED)
            );
    }

    [Fact]
    public async Task ErrorExpenseNotFound()
    {
        var loggedUser = UserBuilder.Build();
        var request = RequestExpenseJsonBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(id: 1000, request);
        var result = await act.ShouldThrowAsync<NotFoundException>();
        var errors = result.GetErrors();

        result.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND)
            );
    }
    
    private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = ExpensesRepositoryBuilder.Build(user, expense: expense);
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggerUser = LoggedUserBuilder.Build(user);

        return new UpdateExpenseUseCase(repository, unitOfWork, mapper, loggerUser);
    }
}

