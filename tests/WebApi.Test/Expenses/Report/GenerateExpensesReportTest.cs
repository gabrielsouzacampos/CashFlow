using Shouldly;
using System.Net;
using System.Net.Mime;

namespace WebApi.Test.Expenses.Report;

public class GenerateExpensesReportTest : CashFlowClassFixture
{
    private const string _method = "api/Report";
    private readonly string _tokenAdmin = string.Empty;
    private readonly string _tokenTeamMember = string.Empty;
    private readonly DateTime _expenseDate;

    public GenerateExpensesReportTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _tokenAdmin = webApplicationFactory.User_Admin.GetToken();
        _tokenTeamMember = webApplicationFactory.User_Team_Member.GetToken();
        _expenseDate = webApplicationFactory.ExpenseAdmin.GetDate();
    }

    [Fact]
    public async Task SuccessPdf()
    {
        var result = await DoGet($"{_method}/Pdf?month={_expenseDate:Y}", _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.Content.Headers.ContentType!.ShouldNotBeNull();
        result.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Pdf);
    }

    [Fact]
    public async Task SuccessExcel()
    {
        var result = await DoGet($"{_method}/Excel?month={_expenseDate:Y}", _tokenAdmin);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        result.Content.Headers.ContentType!.ShouldNotBeNull();
        result.Content.Headers.ContentType!.MediaType.ShouldBe(MediaTypeNames.Application.Octet);
    }

    [Fact]
    public async Task ErrorForbiddenUserNotAllowedPdf()
    {
        var result = await DoGet($"{_method}/Pdf?month={_expenseDate}", _tokenTeamMember);
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task ErrorForbiddenUserNotAllowedExcel()
    {
        var result = await DoGet($"{_method}/Excel?month={_expenseDate}", _tokenTeamMember);
        result.StatusCode.ShouldBe(HttpStatusCode.Forbidden);
    }
}
