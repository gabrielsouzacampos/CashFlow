using CashFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Domain.Entities;

[Table("expenses")]
public class Expense
{
    [Column("id")]
    public long Id { get; set; }

    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("amount")]
    public decimal Amount { get; set; }

    [Column("payment_type")]
    public PaymentType PaymentType { get; set; }

    public ICollection<Tag> Tags { get; set; } = [];

    public long UserId { get; set; }

    public User User { get; set; } = default!;
}
