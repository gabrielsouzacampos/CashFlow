using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(CashFlowDbContext context) : IExpensesRepository
{

    private readonly CashFlowDbContext _context = context;

    public async Task<List<Expense>> GetAll()
    {
        return await _context.Expenses.AsNoTracking().ToListAsync();
    }

    public async Task Add(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }

    public async Task<Expense?> GetById(long id)
    {
        return await _context.Expenses.AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id.Equals(id));
    }

    public async Task<Expense?> GetExpenseById(long id)
    {
        return await _context.Expenses
            .FirstOrDefaultAsync(expense => expense.Id.Equals(id));
    }

    public async Task<bool> DeleteById(long id)
    {
        var result = await _context.Expenses
            .FirstOrDefaultAsync(expense => expense.Id.Equals(id));

        if (result is null)
            return false;

        _context.Expenses.Remove(result);

        return true;
    }

    public void Update(Expense expense)
    {
        _context.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date.ToUniversalTime();
        var endDate = new DateTime(
            year: date.Year,
            month: date.Month,
            day: DateTime.DaysInMonth(year: date.Year, month: date.Month),
            hour: 23,
            minute: 59,
            second: 59,
            DateTimeKind.Utc
        );

        return await _context
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }
}
