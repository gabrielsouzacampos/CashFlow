using CashFlow.Domain.Enums;
using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetById;

public class GetExpenseByIdTest : CashFlowClassFixture
{
    private const string _method = "api/Expenses";
    private readonly string _token = string.Empty;
    private readonly long _expenseId;

    public GetExpenseByIdTest(CustomWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.ExpenseMemberTeam.GetId();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet($"{ _method }/{ _expenseId }", _token);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.RootElement.GetProperty("id").GetInt64().ShouldBe(_expenseId);
        response.RootElement.GetProperty("title").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().ShouldNotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("amount").GetDecimal().ShouldBeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).ShouldBeTrue();
    }
}
