using CashFlow.Domain.Entities;

namespace WebApi.Test.Resources;

public class ExpenseIdentityManager(Expense expense)
{
    private readonly Expense _expense = expense;

    public long GetId() => _expense.Id;
}
