using Expenses.Domain.Entities;

namespace Expenses.Domain.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<User> GetUserByIdAsync(int id, CancellationToken cancellationToken);
}