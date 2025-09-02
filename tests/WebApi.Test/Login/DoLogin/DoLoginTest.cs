using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CommomTestUtilities.Requests;
using Shouldly;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;

public class DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : IClassFixture<CustomWebApplicationFactory>
{
    private const string _method = "api/Login";
    private readonly HttpClient _httpClient = webApplicationFactory.CreateClient();
    private readonly string _email = webApplicationFactory.GetEmail();
    private readonly string _name = webApplicationFactory.GetName();
    private readonly string _password = webApplicationFactory.GetPassword();

    [Fact]
    public async Task Success()
    {
        var request = new RequestLoginJson 
        {
            Email = _email,
            Password = _password
        };

        var response = await _httpClient.PostAsJsonAsync(_method, request);
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);
        responseData.RootElement.GetProperty("name").GetString().ShouldBe(_name);
        responseData.RootElement.GetProperty("token").GetString().ShouldNotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Login_Is_Invalid(string cultureInfo)
    {
        var request = RequestLoginJsonBuilder.Build();
        _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(cultureInfo));

        var response = await _httpClient.PostAsJsonAsync(_method, request);
        var responseBody = await response.Content.ReadAsStreamAsync();
        var responseData = await JsonDocument.ParseAsync(responseBody);
        var errors = responseData.RootElement.GetProperty("errors").EnumerateArray();
        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(cultureInfo));

        response.StatusCode.ShouldBe(HttpStatusCode.Unauthorized);
        errors.ShouldSatisfyAllConditions(
            () => errors.ShouldHaveSingleItem(),
            () => errors.ShouldContain(error => error.GetString()!.Equals(expectedMessage))
        );
    }
}
