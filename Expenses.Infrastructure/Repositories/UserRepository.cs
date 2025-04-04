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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly ExpensesDbContext _dbContext;

        public UserRepository(ExpensesDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _dbContext.Users
                .Include(u => u.Currency)
                .FirstOrDefaultAsync(u => u.IdUser == id, cancellationToken);

            if (user == null)
            {
                throw new EntityNotFoundException($"user with ID {id} not found.");
            }
            return user;
        }
    }
}
