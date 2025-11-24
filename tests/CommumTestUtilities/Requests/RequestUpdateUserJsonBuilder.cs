using Bogus;
using CashFlow.Communication.Requests;

namespace CommomTestUtilities.Requests;

public class RequestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build()
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(u => u.Name, f => f.Person.FirstName)
            .RuleFor(u => u.Email, f => f.Internet.Email());
    }
}

