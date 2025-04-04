namespace Expenses.Domain.Repositories;

public interface IRepositoryBase<TEntity> : IDisposable where TEntity : class
{
    Task AddAsync(TEntity entity, CancellationToken cancellationToken);

    void Add(TEntity entity);
    Task<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task<IList<TEntity>> GetAllAsync(CancellationToken cancellationToken);
    void Update(TEntity entity);
    void Remove(TEntity entity);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
