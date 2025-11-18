using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Expenses.GetById;

public class GetExpenseByIdTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);
        var result = await useCase.Execute(expense.Id);

        result.ShouldNotBeNull();
        result.Id.ShouldBe(expense.Id);
        result.Title.ShouldBe(expense.Title);
        result.Description.ShouldBe(expense.Description);
        result.Date.ShouldBe(expense.Date);
        result.Amount.ShouldBe(expense.Amount);
        result.PaymentType.ShouldBe((PaymentType)expense.PaymentType);
    }

    [Fact]
    public async Task ErrorExpenseNotFound()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser, new Expense());
        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.ShouldThrowAsync<NotFoundException>();
        var errors = result.GetErrors();

        result.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND)
            );
    }

    private static GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = ExpensesRepositoryBuilder.Build(user, expense: expense);
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetExpenseByIdUseCase(repository, mapper, loggedUser);
    }
}
