using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Repositories;

internal class UsersRepository(CashFlowDbContext context) : IUsersRepository
{
    private readonly CashFlowDbContext _context = context;

    public async Task<bool> ExistActiveUserWithEmail(string email) 
        => await _context.Users.AnyAsync(user => user.Email.Equals(email));

    public async Task Add(User user) 
        => await _context.Users.AddAsync(user);

    public async Task<User?> GetUserByEmail(string email)
    {
        var user = await _context.Users.AsNoTracking()
            .FirstOrDefaultAsync(user => user.Email.Equals(email)); 
        return user;
    }
}
