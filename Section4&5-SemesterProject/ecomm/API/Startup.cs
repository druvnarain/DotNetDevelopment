using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Errors;
using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Database;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        //Constructor
        //appsettings.json is used in configureation through dependency injection
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        //public IConfiguration Configuration { get; }


        //dependency injection container for use throughout app
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /* Moved to ApplicationServicesExtension
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped(typeof(IGenericRepository<>), (typeof(GenericRepository<>)));
            */
            services.AddAutoMapper(typeof(MappingProfiles));
            services.AddControllers();
            services.AddDbContext<StoreContext>(x => 
                x.UseSqlite(_configuration.GetConnectionString("DefaultConnection")));
            services.AddApplicationServices();
            services.AddSwaggerDocumentation();
            services.AddCors(option => 
            {
                option.AddPolicy("CorsPolicy", policy =>
                {
                     policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });              
            });
            /* Moved to ApplicationServicesExtension
            //used to configure [ApiController] attributed used in controllers
            services.Configure<ApiBehaviorOptions>(options => 
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                        .Where(e => e.Value.Errors.Count > 0)
                        .SelectMany(x => x.Value.Errors)
                        .Select(x => x.ErrorMessage).ToArray();

                    var errorResponse = new ApiValidationErrorResponse
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(errorResponse);
                };
            });
               Added to SwaggerExtention*/
            // services.AddSwaggerGen(configure =>
            // {
            //     //name must match SwaggerEndpoint url in Configure
            //     configure.SwaggerDoc("v1", new OpenApiInfo {Title = "EComm API", Version = "Version1"});
            // });
        }


        //middleware goes here
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /* Replaced by ExceptionMiddleware.cs
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            } */

            app.UseMiddleware<ExceptionMiddleware>();

            //used for 404 errors, redirects to ErrorController
            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            //if https is available, use it if regular http is entered
            app.UseHttpsRedirection();

            //gets us tp controller, routing functionality
            app.UseRouting();

            //get api to server static files (pics) in wwwroot
            app.UseStaticFiles();

            app.UseCors("CorsPolicy");
            
            app.UseAuthorization();

            // Moved to SwaggerServiceExtension UseSwaggerDocumentation method
            // app.UseSwagger();
            // app.UseSwaggerUI(configuration => {configuration.SwaggerEndpoint("/swagger/v1/swagger.json", "EComm API v1");});
            app.UseSwaggerDocumentation();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
