using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using Union.Backend.Model.DAO;
using Union.Backend.Service.Services;
using static Union.Backend.API.Program;

namespace Union.Backend.API
{
    enum DbContextConfig
    {
        Local,
        NoLocal
    }

    public class Startup
    {
        private readonly DbContextConfig dbContextConfig;
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            if (!Enum.TryParse(Configuration.GetValue<string>(DB_CONTEXT_ARG), true, out dbContextConfig))
                dbContextConfig = DbContextConfig.Local;
            Console.WriteLine($"DbContext in {dbContextConfig} mode");
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
            services.AddControllers(opt =>
            {
                opt.Filters.Add(new HttpResponseExceptionFilter());
            });

            services.AddDbContext<GardenLinkContext>(opt =>
                {
                    switch (dbContextConfig)
                    {
                        case DbContextConfig.Local: opt.UseInMemoryDatabase("LocalList"); break;
                        default: opt.UseMySql(Configuration.GetConnectionString("GardenLinkContext")); break;
                    }
                }
            );

            services.AddSwaggerGen(sd =>
            {
                sd.SwaggerDoc("v1", new OpenApiInfo { Title = "GardenLink", Version = "v1" });
            });

            services.AddTransient<FriendsService, FriendsService>();
            services.AddTransient<GardensService, GardensService>();
            services.AddTransient<LocationsService, LocationsService>();
            services.AddTransient<PaymentsService, PaymentsService>();
            services.AddTransient<ScoresService, ScoresService>();
            services.AddTransient<TalksService, TalksService>();
            services.AddTransient<UsersService, UsersService>();
            services.AddTransient<WalletsService, WalletsService>();
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GardenLink v1");
            });
        }
    }
}
