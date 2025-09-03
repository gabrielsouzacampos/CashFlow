using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Colors;
using CashFlow.Application.UseCases.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Extensions;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCases.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private readonly IExpensesRepository _expensesRepository;
    private readonly ILoggedUser _loggedUser;

    public GenerateExpensesReportPdfUseCase(IExpensesRepository expensesRepository, ILoggedUser loggedUser)
    {
        _expensesRepository = expensesRepository;
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
        _loggedUser = loggedUser;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var loggedUser = await _loggedUser.Get();
        var expenses = await _expensesRepository.FilterByMonth(month, loggedUser);

        if (expenses.Count.Equals(0)) return [];

        var document = CreateDocument(month, loggedUser.Name);

        var page = CreatePage(document);

        CreateHeader(page, loggedUser.Name);

        var totalExpenses = expenses.Sum(expenses => expenses.Amount);

        CreateTotalExpenseSection(page, totalExpenses, month);

        foreach (var expense in expenses) 
        {
            var table = CreateExpenseTable(page);

            var row = table.AddRow();
            row.Height = 25;

            AddExpenseTitle(row.Cells[0], expense.Title);

            AddHeaderForAmount(row.Cells[3]);

            row = table.AddRow();
            row.Height = 25;

            row.Cells[0].AddParagraph(expense.Date.ToString("D"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            row.Cells[0].Format.LeftIndent = 20;

            row.Cells[1].AddParagraph(expense.Date.ToString("t"));
            SetStyleBaseForExpenseInformation(row.Cells[0]);
            
            row.Cells[2].AddParagraph(expense.PaymentType.PaymentsTypeToString());
            SetStyleBaseForExpenseInformation(row.Cells[2]);

            AddValueForExpense(row.Cells[3], expense.Amount);

            if (!string.IsNullOrWhiteSpace(expense.Description))
            {
                var descriptionRow = table.AddRow();
                descriptionRow.Height = 25;

                descriptionRow.Cells[0].AddParagraph(expense.Description);
                descriptionRow.Cells[0].Shading.Color = ColorHelper.GREEN_LIGTH;
                descriptionRow.Cells[0].VerticalAlignment = VerticalAlignment.Center;
                descriptionRow.Cells[0].MergeRight = 2;
                descriptionRow.Cells[0].Format.LeftIndent = 20;
                descriptionRow.Cells[0].Format.Font = new Font
                {
                    Name = FontHelper.WORKSANS_REGULAR,
                    Size = 10,
                    Color = ColorHelper.BLACK,
                };

                row.Cells[3].MergeDown = 1;
            }

            AddWhiteSpace(table);
        }

        return RenderDocument(document);
    }

    private static Document CreateDocument(DateOnly month, string author)
    {
        var document = new Document
        {
            Info = new DocumentInfo
            {
                Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}",
                Author = author
            }
        };

        document.Styles["normal"]!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private static Section CreatePage(Document document)
    {
        var section = document.AddSection();

        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private static void CreateHeader(Section page, string name)
    {
        var header = page.AddParagraph();
        header.AddFormattedText($"Hey, { name }",
            new Font { Name = FontHelper.RALEWAY_BLACK, Size = 16 });
    }

    private static void CreateTotalExpenseSection(Section page, decimal totalExpenses, DateOnly month)
    {
        var paragraph = page.AddParagraph();
        paragraph.Format.SpaceBefore = "40";
        paragraph.Format.SpaceAfter = "40";

        var title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(title,
            new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15 });

        paragraph.AddLineBreak();


        paragraph.AddFormattedText($"R$ {totalExpenses}",
            new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
    }

    private static Table CreateExpenseTable(Section page)
    {
        var table = page.AddTable();

        var column = table.AddColumn("195")
            .Format.Alignment = ParagraphAlignment.Left;
        table.AddColumn("80")
            .Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120")
            .Format.Alignment = ParagraphAlignment.Center;
        table.AddColumn("120")
            .Format.Alignment = ParagraphAlignment.Right;


        return table;
    }

    private static void AddExpenseTitle(Cell cell, string title)
    {
        cell.AddParagraph(title);
        cell.Shading.Color = ColorHelper.RED_LIGTH;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.MergeRight = 2;
        cell.Format.LeftIndent = 20;
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorHelper.BLACK,
        };
    }

    private static void AddHeaderForAmount(Cell cell)
    {
        cell.AddParagraph(ResourceReportGenerationMessages.AMOUNT);
        cell.Shading.Color = ColorHelper.RED_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font
        {
            Name = FontHelper.RALEWAY_BLACK,
            Size = 14,
            Color = ColorHelper.WHITE,
        };
    }

    private static void SetStyleBaseForExpenseInformation(Cell cell)
    {
        cell.Shading.Color = ColorHelper.GREEN_DARK;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 12,
            Color = ColorHelper.BLACK,
        };
    }

    private static void AddValueForExpense(Cell cell, decimal amount)
    {
        cell.AddParagraph($"- R$ {amount}");
        cell.Shading.Color = ColorHelper.WHITE;
        cell.VerticalAlignment = VerticalAlignment.Center;
        cell.Format.Font = new Font
        {
            Name = FontHelper.WORKSANS_REGULAR,
            Size = 14,
            Color = ColorHelper.BLACK,
        };
    }

    private static void AddWhiteSpace(Table table)
    {
        var row = table.AddRow();
        row.Height = 30;
        row.Borders.Visible = false;
    }

    private static byte[] RenderDocument(Document document)
    {
        var render = new PdfDocumentRenderer
        {
            Document = document,
        };

        render.RenderDocument();

        using var file = new MemoryStream();
        render.PdfDocument.Save(file);

        return file.ToArray();
    }
}
