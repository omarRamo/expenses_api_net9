using Expenses.Domain.Entities;
using Expenses.Utils.Enum;

namespace Expenses.Domain.Repositories;

public interface IExpenseRepository : IRepositoryBase<Expense>
{
    Task<Expense> GetExpenseByDateAndAmountAsync(int expenseId, DateTime expenseDate, decimal amount);
    Task<Expense> GetExpenseByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId);
    Task<IEnumerable<Expense>> GetExpensesByUserIdSortedAsync(int? userId, SortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken);
}
