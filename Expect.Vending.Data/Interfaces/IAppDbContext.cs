using Expect.Vending.Domain.Interfaces;
using Expect.Vending.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Runtime.CompilerServices;

namespace Expect.Vending.Data.Interfaces
{
    public interface IAppDbContext : IDisposable
    {
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<TEntity> Set<TEntity>() where TEntity : class;


        public EntityEntry Entry(object entity);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);


    }
}
