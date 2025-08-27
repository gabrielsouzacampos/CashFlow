using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesRepository
{
    Task Add(Expense expense);
    
    Task<List<Expense>> GetAll();
    
    Task<Expense?> GetById(long id);

    Task<Expense?> GetExpenseById(long id);

    Task<List<Expense>> FilterByMonth(DateOnly date);

    void Update(Expense expense);
    
    /// <summary>
    /// This function returns TRUE if the deletion was successful otherwise returns FALSE.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> DeleteById(long id);
}
