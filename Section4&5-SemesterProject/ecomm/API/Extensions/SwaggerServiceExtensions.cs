using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddSwaggerGen(configure =>
            {
                //name must match SwaggerEndpoint url in Configure
                configure.SwaggerDoc("v1", new OpenApiInfo {Title = "EComm API", Version = "Version1"});
            });
            return services;
        }

        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app) 
        {
            app.UseSwagger();
            app.UseSwaggerUI(configuration => {configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "EComm API v1");});
            return app;
        }
    }
}