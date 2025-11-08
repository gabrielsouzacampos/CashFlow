using CashFlow.Application.UseCases.Expenses.GetAll;
using CashFlow.Domain.Entities;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using Shouldly;

namespace UseCases.Test.Expenses.GetAll;

public class GetAllExpensesUseCaseTest
{
    [Fact]
    public async Task Success()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(loggedUser, expenses);

        var result = await useCase.Execute();

        result.ShouldNotBeNull();
        result.Expenses.ShouldSatisfyAllConditions(expense =>
        {
            foreach (var item in expense)
            {
                item.Id.ShouldBeGreaterThan(0);
                item.Title.ShouldNotBeNullOrEmpty();
                item.Amount.ShouldBeGreaterThan(0);
            }
        });
    }

    private GetAllExpensesUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = ExpensesRepositoryBuilder.Build(user, expenses);
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetAllExpensesUseCase(repository, mapper, loggedUser);
    }
}
