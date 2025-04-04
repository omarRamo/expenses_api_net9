using Expenses.Domain.Entities;
using Expenses.Domain.Exceptions;
using Expenses.Domain.Repositories;
using Expenses.Infrastructure.DbContext;
using Expenses.Utils.Enum;
using Microsoft.EntityFrameworkCore;

namespace Expenses.Infrastructure.Repositories;

public class ExpenseRepository: RepositoryBase<Expense>, IExpenseRepository
{
    private readonly ExpensesDbContext _dbContext;
    public ExpenseRepository(ExpensesDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Expense> GetExpenseByDateAndAmountAsync(int expenseId, DateTime expenseDate, decimal amount)
    {
        var expense = await _dbContext.Expenses
            .Where(e => e.UserId == expenseId && e.ExpenseDate.Date == expenseDate.Date && e.Amount == amount)
            .FirstOrDefaultAsync();

        return expense;
    }

    public async Task<Expense> GetExpenseByIdAsync(int id, CancellationToken cancellationToken)
    {
        var expense = await _dbContext.Expenses
            .Include(e => e.Currency)
            .Include(e => e.ExpenseType)
            .Include(e => e.User)
            .FirstOrDefaultAsync(e => e.IdExpense == id, cancellationToken);

        if (expense == null)
        {
            throw new EntityNotFoundException($"user with ID {id} not found.");
        }
        return expense;
    }

    public async Task<IEnumerable<Expense>> GetExpensesByUserIdAsync(int userId)
    {
        IEnumerable<Expense> list = await DbSet
                .Include(e => e.Currency)
                .Include(e => e.ExpenseType)
                .Include(e => e.User)
                .Where(e => e.UserId == userId).ToListAsync();

        return list;
    }

    public async Task<IEnumerable<Expense>> GetExpensesByUserIdSortedAsync(int? userId, SortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken)
    {
        {
            IQueryable<Expense> query = DbSet
                .Include(e => e.Currency)
                .Include(e => e.ExpenseType)
                .Include(e => e.User);

            // Filter by userId if provided
            if (userId.HasValue)
            {
                query = query.Where(e => e.UserId == userId.Value);
            }

            switch (sortBy)
            {
                case SortBy.amount:
                    query = sortOrder == SortOrder.Descending
                        ? query.OrderByDescending(e => e.Amount)  
                        : query.OrderBy(e => e.Amount); 
                    break;

                case SortBy.expenseDate:
                    query = sortOrder == SortOrder.Descending
                        ? query.OrderByDescending(e => e.ExpenseDate)  
                        : query.OrderBy(e => e.ExpenseDate);  
                    break;

                default:
                    query = query.OrderBy(e => e.ExpenseDate);
                    break;
            }

            return await query.ToListAsync(cancellationToken);
        }
    }
}
