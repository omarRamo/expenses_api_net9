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

namespace Expenses.Infrastructure.Repositories
{
    public class CurrencyRespository(ExpensesDbContext context) : RepositoryBase<Expense>(context), ICurrencyRepository
    {
        public async Task<Currency> GetCurrencyByName(string code)
        {
            var currency = await context.Currencies
                                         .FirstOrDefaultAsync(c => c.CurrencyCode.ToLower() == code.ToLower());
            if (currency == null)
            {
                throw new EntityNotFoundException($"Currency with name {code} not found.");
            }
            return currency; ;
        }
    }
}
