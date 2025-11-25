using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommomTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.ChangePassword;

public class ChangePasswordTest : CashFlowClassFixture
{
    private const string _method = "api/User/change-password";
    private readonly string _token;
    private readonly string _email;
    private readonly string _password;

    public ChangePasswordTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _email = webApplicationFactory.User_Team_Member.GetEmail();
        _password = webApplicationFactory.User_Team_Member.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        request.Password = _password;

        var response = await DoPut(_method, request, _token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var loginRequest = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        response = await DoPost("api/login", loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);

        loginRequest.Password = request.NewPassword;

        response = await DoPost("api/login", loginRequest);
        response.StatusCode.ShouldBe(HttpStatusCode.OK);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorPasswordDifferentCurrentPassword(string culture)
    {
        var request = RequestChangePasswordJsonBuilder.Build();
        
        var result = await DoPut(_method, request, _token, culture);

        var body = await result.Content.ReadAsStreamAsync();
        var response = JsonDocument.Parse(body);
        var errors = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("PASSWORD_DIFFERENT_CURRENT_PASSWORD", new CultureInfo(culture));

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
        );
    }
}

