using CashFlow.Exception;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Delete;

public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string _method = "api/Expenses";
    private readonly string _token = string.Empty;
    private readonly long _expenseId;

    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoDelete($"{_method}/{ _expenseId }", _token);
        
        result.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        result = await DoGet($"{_method}/{ _expenseId }", _token);

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorExpenseNotFound(string cultureInfo)
    {
        var result = await DoDelete($"{ _method }/1000", _token, cultureInfo);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));

        result.StatusCode.ShouldBe(HttpStatusCode.NotFound);
        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
            );
    }
}

