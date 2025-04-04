using Expenses.Domain.Entities;
using Expenses.Domain.Exceptions;
using Expenses.Domain.Repositories;
using Expenses.Infrastructure.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Infrastructure.Repositories;

public class ExpenseTypeRepository : RepositoryBase<ExpenseType>, IExpenseTypeRepository
{
    private readonly ExpensesDbContext _dbContext;
    public ExpenseTypeRepository(ExpensesDbContext context) : base(context)
    {
        _dbContext = context;
    }

    public async Task<ExpenseType> GetExpenseTypeByLabelAsync(string label, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.ExpenseTypes
            .FirstOrDefaultAsync(e => e.Label.ToLower() == label.ToLower(), cancellationToken);

        if (entity == null)
        {
            throw new EntityNotFoundException($"ExpenseType with label {label} not found.");
        }

        return entity;
    }
}