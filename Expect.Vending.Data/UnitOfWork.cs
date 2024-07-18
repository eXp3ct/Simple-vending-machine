using Expect.Vending.Data.Interfaces;
using Expect.Vending.Data.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Expect.Vending.Data
{
    public class UnitOfWork(IAppDbContext context, ILoggerFactory loggerFactory) : IUnitOfWork
    {
        private readonly IAppDbContext _context = context;
        private readonly ILoggerFactory _loggerFactory = loggerFactory;
        private readonly Dictionary<Type, object> _repositories = [];

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        IRepository<TEntity> IUnitOfWork.Repository<TEntity>()
        {
            if(_repositories.TryGetValue(typeof(TEntity), out var repository))
            {
                return (IRepository<TEntity>)repository;
            }

            var logger = _loggerFactory.CreateLogger<BaseRepository<TEntity>>();
            var newRepository = new BaseRepository<TEntity>(_context, logger);
            _repositories[typeof(TEntity)] = newRepository;

            return newRepository;
        }
    }
}
