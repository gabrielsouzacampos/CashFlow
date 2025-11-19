using CashFlow.Application.UseCases.Expenses.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Expenses.Delete;

public class DeleteExpenseUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(loggedUser);

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(expense.Id);

        await act.ShouldNotThrowAsync();
    }

    [Fact]
    public async Task ErrorExpenseNotFound()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(loggedUser);

        var act = async () => await useCase.Execute(1000);
        var result = await act.ShouldThrowAsync<NotFoundException>();
        var errors = result.GetErrors();

        result.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(ResourceErrorMessages.EXPENSE_NOT_FOUND)
            );
    }

    public DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = ExpensesRepositoryBuilder.Build(user, expense: expense);
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(repository, unitOfWork, loggedUser);
    }
}

