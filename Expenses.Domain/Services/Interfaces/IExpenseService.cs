using Expenses.Domain.Entities;
using Expenses.Utils.Enum;
using System.Globalization;

namespace Expenses.Domain.Services;

public interface IExpenseService
{
    Task<Expense> GetExpenseByIdAsync(int id, CancellationToken cancellationToken);
    Task<int> CreateExpenseAsync(Expense expenseRequest, string currency, string type, CancellationToken cancellationToken);
    Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Expense>> GetExpensesSortedAsync(int? userId, SortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken);
}
