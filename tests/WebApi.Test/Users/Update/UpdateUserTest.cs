using CashFlow.Exception;
using CommomTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update;

public class UpdateUserTest : CashFlowClassFixture
{
    private const string _method = "api/User";
    private readonly string _token;

    public UpdateUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Success()
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        var response = await DoPut(_method, request, _token);

        response.StatusCode.ShouldBe(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task ErrorEmptyName(string culture)
    {
        var request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(_method, request, _token, culture: culture);
        var body = await result.Content.ReadAsStreamAsync();
        var response = JsonDocument.Parse(body);
        var errors = response.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));

        result.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
        );


    }

}

