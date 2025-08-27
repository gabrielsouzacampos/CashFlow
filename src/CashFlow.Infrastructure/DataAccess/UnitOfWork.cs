using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAccess;

internal class UnitOfWork(CashFlowDbContext context) : IUnitOfWork
{
    private readonly CashFlowDbContext _context = context;

    public async Task Commit() => await _context.SaveChangesAsync();
}
