using Expect.Vending.Data;
using Expect.Vending.Infrastructure;
using Expect.Vending.Web.Middlewares;

namespace Expect.Vending.Web
{
    /// <summary>
    /// Конфигурация
    /// </summary>
    /// <param name="configuration"></param>
    public class Startup(IConfiguration configuration)
    {
        /// <summary>
        /// Объект конфигурации
        /// </summary>
        public IConfiguration Configuration { get; set; } = configuration;

        /// <summary>
        /// Настройка http
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseRouting();
            app.UseCors("AllowAllOrigins");
            
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }

        /// <summary>
        /// Настройка DI контейнера
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowAnyOrigin();
                    });
            });
            services.AddControllers().AddNewtonsoftJson();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            SwaggerOptions.Configure(services);
            services.AddPersistance(Configuration);
            services.AddInfrastructure();
            
	    
        }
    }
}
