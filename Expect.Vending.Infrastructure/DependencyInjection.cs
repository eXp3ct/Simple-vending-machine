using Expect.Vending.Infrastructure.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Expect.Vending.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddSingleton<IFileManager, FileManager>();
        }
    }
}
