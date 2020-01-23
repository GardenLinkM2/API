using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Services;

namespace Union.Backend.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });
            services.AddDbContext<GardenLinkContext>(opt =>
               opt.UseInMemoryDatabase("TodoList")
            );
            services.AddSwaggerGen(sd =>
            {
                sd.SwaggerDoc("v1", new OpenApiInfo { Title = "SwaggerDemo", Version = "v1" });
                sd.OrderActionsBy(key => key.HttpMethod switch
                {
                    "GET" => "0",
                    "POST" => "1",
                    "PUT" => "2",
                    "DELETE" => "3",
                    _ => "4",
                });
                sd.DocumentFilter<JustDtoDocumentFilter>();
            });

            services.AddTransient<UsersService, UsersService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "SwaggerDemo v1");
            });
        }
    }
}
