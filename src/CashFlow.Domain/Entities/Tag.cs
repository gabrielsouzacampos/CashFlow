
using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Domain.Entities;

[Table("tags")]
public class Tag
{
    [Column("id")]
    public long Id { get; set; }

    [Column("value")]
    public Enums.Tag Value { get; set; }

    public long ExpenseId { get; set; }

    public Expense Expense { get; set; } = default!;
}
