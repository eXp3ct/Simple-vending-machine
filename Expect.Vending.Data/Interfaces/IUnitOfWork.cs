using Expect.Vending.Domain.Interfaces;

namespace Expect.Vending.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class, IEntity;
    }
}
