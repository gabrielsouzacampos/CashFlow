using System.Globalization;

namespace CashFlow.Api.Middleware;

public class CultureMiddleware(RequestDelegate next)
{
    private RequestDelegate _next { get; set; } = next;

    public async Task Invoke(HttpContext context)
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();
        var requestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();
        var cultureInfo = new CultureInfo("en");

        if (!string.IsNullOrWhiteSpace(requestedCulture)
            && supportedLanguages.Exists(language => language.Name.Equals(requestedCulture)))
        {

            cultureInfo = new CultureInfo(requestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
