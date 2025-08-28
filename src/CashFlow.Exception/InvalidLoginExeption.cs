using System.Net;

namespace CashFlow.Exception;

public class InvalidLoginExeption() : CashFlowException(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID)
{
    public override int StatusCode => (int)HttpStatusCode.Unauthorized;

    public override List<string> GetErrors() => [Message];
}

