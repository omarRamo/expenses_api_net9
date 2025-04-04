using Expenses.Domain.Entities;

namespace Expenses.Domain.Repositories;

public interface IExpenseTypeRepository : IRepositoryBase<ExpenseType>
{
    Task<ExpenseType> GetExpenseTypeByLabelAsync(string label, CancellationToken cancellationToken);

}
