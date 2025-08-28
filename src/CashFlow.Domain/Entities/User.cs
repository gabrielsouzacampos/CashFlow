using CashFlow.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashFlow.Domain.Entities;

[Table("users")]
public class User
{
    [Column("id")]
    public long Id { get; set; }

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("user_identifier")]
    public Guid UserIdentifier { get; set; }

    [Column("role")]
    public string Role { get; set; } = Roles.TEAM_MEMBER;
}
