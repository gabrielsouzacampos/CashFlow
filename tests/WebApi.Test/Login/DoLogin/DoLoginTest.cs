using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommomTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest : CashFlowClassFixture
{
    private const string _method = "api/Login";
    private readonly string _email = string.Empty;
    private readonly string _name = string.Empty;
    private readonly string _password = string.Empty;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.GetEmail();
        _name = webApplicationFactory.GetName();
        _password = webApplicationFactory.GetPassword();
    }

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson 
        {
            Email = _email,
            Password = _password
        };

        var result = await DoPost(_method, request);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);

        result.StatusCode.ShouldBe(HttpStatusCode.OK);
        response.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        response.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Is_Invalid(string cultureInfo)
    {
        var request = RequestLoginJsonBuilder.Build();

        var result = await DoPost(_method, request, culture: cultureInfo);
        var body = await result.Content.ReadAsStreamAsync();
        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(cultureInfo));

        result.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
        );
    }
}
