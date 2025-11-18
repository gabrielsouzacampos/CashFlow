using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Criptografy;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommomTestUtilities.Entities;
using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private User _user = new();
    private Expense _expense = new();

    private string _token = string.Empty;
    private string _password = string.Empty;

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

                StartDatabase(dbContext, passwordEncripter);
                _token = tokenGenerator.GenerateAccessToken(_user);
            });
    }

    public string GetName() => _user.Name;

    public string GetEmail() => _user.Email;

    public string GetPassword() => _password;

    public string GetToken() => _token;

    public long GetExpenseId() => _expense.Id;

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        AddUsers(dbContext, passwordEncripter);
        AddExpenses(dbContext, _user);
        dbContext.SaveChanges();
    }

    private void AddUsers(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = passwordEncripter.Encrypt(_user.Password);

        dbContext.Users.Add(_user);
    }

    private void AddExpenses(CashFlowDbContext dbContext, User user)
    {
        _expense = ExpenseBuilder.Build(user);

        dbContext.Expenses.Add(_expense);
    }
}
