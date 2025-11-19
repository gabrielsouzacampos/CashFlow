using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommomTestUtilities.Repositories;

public class ExpensesRepositoryBuilder
{
    public static IExpensesRepository Build(User? user = null, List<Expense>? expenses = null, Expense? expense = null)
    {
        var mock = new Mock<IExpensesRepository>();

        if (user != null && expenses != null)
            mock.Setup(repository => repository.GetAll(user)).ReturnsAsync(expenses);

        if (user != null && expense != null)
        {
            mock.Setup(repository => repository.GetById(expense.Id, user)).ReturnsAsync(expense);
            mock.Setup(repository => repository.GetExpenseById(expense.Id, user)).ReturnsAsync(expense);
        }

        return mock.Object;
    }
}
