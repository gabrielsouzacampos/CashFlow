using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Expenses.GetAll;

public class GetAllExpensesTest : CashFlowClassFixture
{
    private const string _method = "api/Expenses";
    private readonly string _token = string.Empty;

    public GetAllExpensesTest(CustomWebApplicationFactory webApplicationFactory)
        : base(webApplicationFactory) => _token = webApplicationFactory.User_Team_Member.GetToken();

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(_method, _token);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.RootElement.GetProperty("expenses").EnumerateArray().ShouldNotBeEmpty();
    }
}
