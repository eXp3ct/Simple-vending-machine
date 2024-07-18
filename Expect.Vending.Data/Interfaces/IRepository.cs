using Expect.Vending.Domain.Interfaces;

namespace Expect.Vending.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        public Task<TEntity?> Add(TEntity entity, CancellationToken cancellationToken = default);
        public Task<TEntity?> Update(Guid id, TEntity newEntity, CancellationToken cancellationToken = default);
        public Task<TEntity?> Delete(Guid id, CancellationToken cancellationToken = default);
        public Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default);
        public Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default);
    }
}
