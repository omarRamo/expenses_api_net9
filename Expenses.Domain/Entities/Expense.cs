namespace Expenses.Domain.Entities;

public class Expense
{
    public int IdExpense { get; set; }
    public DateTime ExpenseDate { get; set; }
    public string? Comment { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int CurrencyId { get; set; }
    public Currency Currency { get; set; }
    public decimal Amount { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
    public int ExpenseTypesId { get; set; }
    public ExpenseType ExpenseType { get; set; }
}
