using Shouldly;
using System.Net;
using System.Text.Json;

namespace WebApi.Test.Users.Profile;

public class GetUserProfileTest : CashFlowClassFixture
{
    private const string _method = "api/User";

    private readonly string _token = string.Empty;
    private readonly string _userName;
    private readonly string _userEmail;

    public GetUserProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _userName = webApplicationFactory.User_Team_Member.GetName();
        _userEmail = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Success()
    {
        var result = await DoGet(_method, _token);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.RootElement.GetProperty("name").GetString().ShouldBe(_userName);
        response.RootElement.GetProperty("email").GetString().ShouldBe(_userEmail);
    }
}
