using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesRepository
{
    Task Add(Expense expense);
    
    Task<List<Expense>> GetAll(User user);
    
    Task<Expense?> GetById(long id, User user);

    Task<Expense?> GetExpenseById(long id, User user);

    Task<List<Expense>> FilterByMonth(DateOnly date, User user);

    void Update(Expense expense);
    
    Task DeleteById(long id);
}
