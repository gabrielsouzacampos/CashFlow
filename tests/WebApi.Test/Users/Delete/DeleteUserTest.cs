using Shouldly;
using System.Net;

namespace WebApi.Test.Users.Delete;

public class DeleteUserTest : CashFlowClassFixture
{
    private const string _method = "api/User";
    private readonly string _token;

    public DeleteUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var response = await DoDelete(_method, _token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }
}

