using Bogus;
using CashFlow.Communication.Requests;

namespace CommomTestUtilities.Requests;

public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password(memorable: false, prefix: "!Aa1"))
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(memorable: false, prefix: "!Aa1"))
            .Generate();
    }
}
