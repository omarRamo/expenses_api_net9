using Expenses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expenses.Domain.Repositories
{
    public interface ICurrencyRepository
    {
        Task<Currency> GetCurrencyByName(string name);
    }
}
