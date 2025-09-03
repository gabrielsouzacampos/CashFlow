using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class ExpensesRepository(CashFlowDbContext context) : IExpensesRepository
{

    private readonly CashFlowDbContext _context = context;

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _context.Expenses.AsNoTracking()
            .Where(expense => expense.UserId.Equals(user.Id)).ToListAsync();
    }

    public async Task Add(Expense expense)
    {
        await _context.Expenses.AddAsync(expense);
    }

    public async Task<Expense?> GetById(long id, User user)
    {
        return await _context.Expenses.AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id.Equals(id) && expense.UserId.Equals(user.Id));
    }

    public async Task<Expense?> GetExpenseById(long id, User user)
    {
        return await _context.Expenses
            .FirstOrDefaultAsync(expense => expense.Id.Equals(id) && expense.UserId.Equals(user.Id));
    }

    public async Task DeleteById(long id)
    {
        var result = await _context.Expenses
            .FirstAsync(expense => expense.Id.Equals(id));

        _context.Expenses.Remove(result);
    }

    public void Update(Expense expense)
    {
        _context.Expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date, User user)
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
            .Where(expense => expense.Date >= startDate && expense.Date <= endDate && expense.UserId.Equals(user.Id))
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title)
            .ToListAsync();
    }
}
