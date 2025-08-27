using PdfSharp.Fonts;
using System.Reflection;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;

public class ExpensesReportFontResolver : IFontResolver
{
    public byte[]? GetFont(string faceName)
    {
        var stream = ReadFontFile(faceName) 
            ?? ReadFontFile(FontHelper.DEFAULT_FONT);

        var data = new byte[stream!.Length];

        stream.ReadExactly(data, 0, (int)stream!.Length);

        return data;
    }

    public FontResolverInfo? ResolveTypeface(string familyName, bool bold, bool italic) 
        => new(familyName);

    private static Stream? ReadFontFile(string faceName)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var test = $"CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts.{faceName}.ttf";

        return assembly.GetManifestResourceStream(test);
    }
}

