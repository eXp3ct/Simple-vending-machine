using Expect.Vending.Data.Contexts;
using Expect.Vending.Data.Interfaces;
using Expect.Vending.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Expect.Vending.Data.Repositories
{
    public class BaseRepository<TEntity>(IAppDbContext context, ILogger<BaseRepository<TEntity>> logger) 
        : IRepository<TEntity> where TEntity : class, IEntity
    {
        private readonly DbSet<TEntity> _set = context.Set<TEntity>();
        private readonly ILogger<BaseRepository<TEntity>> _logger = logger;
        private readonly IAppDbContext _context = context;
        private readonly string _entityName = typeof(TEntity).Name;

        public async Task<TEntity?> Add(TEntity entity, CancellationToken cancellationToken = default)
        {
            var entry = await _set.AddAsync(entity, cancellationToken);

            _logger.LogInformation("{type} : {id} was added to database", _entityName, entity.Id);


            return entry.Entity;
        }

        public async Task<TEntity?> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _set.FindAsync([id, cancellationToken], cancellationToken);

            if (entity is null) return null;

            var entry = _set.Remove(entity);

            _logger.LogInformation("{type} : {id} was deleted from database", _entityName, entity.Id);

            return entry.Entity;
        }

        public async Task<IEnumerable<TEntity>> GetAll(CancellationToken cancellationToken = default)
        {
            var entities = await _set.ToListAsync(cancellationToken);

            _logger.LogInformation("Returning {count} of {type}", entities.Count, _entityName);

            return entities;
        }

        public async Task<TEntity?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var entity = await _set.FindAsync([id, cancellationToken], cancellationToken);

            if (entity is null) return null;

            _logger.LogInformation("{type} : {id} found in database", _entityName, entity.Id);

            return entity;
        }

        public async Task<TEntity?> Update(Guid id, TEntity newEntity, CancellationToken cancellationToken = default)
        {
            var entity = await _set.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);

            if (entity is null) return null;

            newEntity.Id = id;
            _context.Entry(entity).CurrentValues.SetValues(newEntity);

            _logger.LogInformation("{type} : {id} was updated in database", _entityName, entity.Id);

            return entity;
        }


    }
}
