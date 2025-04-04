namespace Expenses.Domain.Entities;

public class ExpenseType
{
    public int IdTypeExpenses { get; set; }
    public string Label { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public ICollection<Expense> Expenses { get; set; }
}
