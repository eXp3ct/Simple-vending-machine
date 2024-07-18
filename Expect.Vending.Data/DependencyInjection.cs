using Expect.Vending.Data.Contexts;
using Expect.Vending.Data.Interfaces;
using Expect.Vending.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Expect.Vending.Data
{
    public static class DependencyInjection
    {
        public static void AddPersistance(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IAppDbContext, AppDbContext>();
            services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
