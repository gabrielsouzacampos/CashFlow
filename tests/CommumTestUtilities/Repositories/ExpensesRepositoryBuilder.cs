using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommomTestUtilities.Repositories;

public class ExpensesRepositoryBuilder
{
    public static IExpensesRepository Build()
    {
        var mock = new Mock<IExpensesRepository>();

        return mock.Object;
    }
}
