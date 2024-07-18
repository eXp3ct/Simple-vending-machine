using Expect.Vending.Data.Interfaces;
using Expect.Vending.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Expect.Vending.Data.Contexts
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<Image> Images { get; set; }

        public AppDbContext() { }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
