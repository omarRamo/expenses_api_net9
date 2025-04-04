namespace Expenses.Domain.Entities;

public class Currency
{
    public int IdCurrency { get; set; }
    public string CurrencyCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public ICollection<User> Users { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}
