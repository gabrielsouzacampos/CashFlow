using System.Net;

namespace CashFlow.Exception;

public class ErrorOnValidationException(List<string> errors) : CashFlowException(string.Empty)
{
    public override int StatusCode => (int)HttpStatusCode.BadRequest;

    public override List<string> GetErrors() => errors;
}

