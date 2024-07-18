using Expect.Vending.Domain.Dtos;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Expect.Vending.Web
{
    /// <summary>
    /// Конфигурация Swagger
    /// </summary>
    public static class SwaggerOptions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        public static void Configure(IServiceCollection services)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Vending Machine Web API",
                    Contact = new()
                    {
                        Email = "roma.veselov.1990@mail.ru",
                        Name = "Roman Veselov",
                        Url = new Uri("https://vk.com/exp3cted")
                    },
                    Description = "Vending machine",
                    Version = "v1"
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);
                options.OperationFilter<AddFileUploadOperationFilter>();
            });
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class AddFileUploadOperationFilter : IOperationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="context"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(ImageDto));

            if (fileParams.Any())
            {
                operation.Parameters.Clear();
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = new Dictionary<string, OpenApiMediaType>
                    {
                        ["multipart/form-data"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties = new Dictionary<string, OpenApiSchema>
                                {
                                    ["DrinkId"] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "uuid"
                                    },
                                    ["ImageFile"] = new OpenApiSchema
                                    {
                                        Type = "string",
                                        Format = "binary"
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
