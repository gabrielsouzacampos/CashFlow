using System.Net;

namespace CashFlow.Exception;

public class NotFoundException(string message) : CashFlowException(message)
{
    public override int StatusCode => (int)HttpStatusCode.NotFound;

    public override List<string> GetErrors() => [message];
}

