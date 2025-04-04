using Expenses.Application.Dtos;
using Expenses.Utils.Enum;

namespace Expenses.Application.Facades.Interfaces;

public interface IExpenseFacade
{
    Task<int> CreateExpenseAsync(ExpenseRequestDto expenseRequestDto, CancellationToken cancellationToken);
    Task<ExpenseResponseDto> GetExpenseByIdAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<ExpenseResponseDto>> GetExpensesSortedAsync(int? userId, SortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken);
    Task<IEnumerable<ExpenseResponseDto>> GetExpensesByUserIdAsync(int id, CancellationToken cancellationToken);
}
