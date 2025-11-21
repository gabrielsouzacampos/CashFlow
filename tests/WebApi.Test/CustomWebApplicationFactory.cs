using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Criptografy;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommomTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    public ExpenseIdentityManager ExpenseMemberTeam { get; private set; } = default!;

    public ExpenseIdentityManager ExpenseAdmin { get; private set; } = default!;

    public UserIdentityManager User_Team_Member { get; private set; } = default!;

    public UserIdentityManager User_Admin { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter, tokenGenerator);
            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, tokenGenerator);
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, tokenGenerator);

        ExpenseMemberTeam = new ExpenseIdentityManager(AddExpenses(dbContext, userTeamMember, 1));
        ExpenseAdmin = new ExpenseIdentityManager(AddExpenses(dbContext, userAdmin, 2));

        dbContext.SaveChanges();
    }

    private User AddUserAdmin(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);
        var token = tokenGenerator.GenerateAccessToken(user);
        user.Id = 2;

        dbContext.Users.Add(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;
    }

    private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAccessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();
        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);
        var token = tokenGenerator.GenerateAccessToken(user);

        dbContext.Users.Add(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;
    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {
        var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        dbContext.Expenses.Add(expense);

        return expense;
    }
}
