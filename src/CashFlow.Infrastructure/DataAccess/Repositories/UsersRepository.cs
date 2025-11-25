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

    public async Task<User?> GetById(long id) 
        => await _context.Users.FirstOrDefaultAsync(user => user.Id.Equals(id));

    public void Update(User user) => _context.Users.Update(user);

    public async void Delete(User user)
    {
        var userToDelete = await _context.Users.FindAsync(user.Id);

        _context.Users.Remove(userToDelete!);
    }
}
