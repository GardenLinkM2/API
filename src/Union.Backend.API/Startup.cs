﻿using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNet.OData.Formatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using Union.Backend.Model;
using Union.Backend.Model.DAO;
using Union.Backend.Service;
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
            services.AddCors(opt => opt.AddPolicy("LeMoulinPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(Service.Auth.AuthorizeAttribute), 255);
                options.EnableEndpointRouting = false;

                foreach (var outputFormatter in options.OutputFormatters.OfType<ODataOutputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    outputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
                foreach (var inputFormatter in options.InputFormatters.OfType<ODataInputFormatter>().Where(_ => _.SupportedMediaTypes.Count == 0))
                {
                    inputFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/prs.odatatestxx-odata"));
                }
            })
            .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
            .AddJsonOptions(opt =>
            {
                opt.JsonSerializerOptions.IgnoreNullValues = true;
                opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            services.AddOData();

            services.AddMvcCore(options => 
            {
                options.OutputFormatters.OfType<ODataOutputFormatter>().ToList().ForEach(o => options.OutputFormatters.Remove(o));
                options.InputFormatters.OfType<ODataInputFormatter>().ToList().ForEach(i => options.InputFormatters.Remove(i));
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
                    default: 
                        opt.UseMySql(Configuration.GetConnectionString("GardenLinkContext"), 
                                     mySqlOptions =>
                                     {
                                         mySqlOptions.EnableRetryOnFailure(10, TimeSpan.FromSeconds(30), null);
                                     });
                        break;
                }
            });

            services.AddSwaggerGen(sd =>
            {
                sd.SwaggerDoc("v1", new OpenApiInfo { Title = "GardenLink", Version = "v1" });
                sd.SchemaFilter<SwaggerExcludeSchemaFilter>();
                sd.OperationFilter<AdditionalHeaderFilter>();
                sd.DocumentFilter<JustDtoDocumentFilter>();
            });

            services.AddTransient<ClientDialogService, ClientDialogService>();
            services.AddTransient<ContactsService, ContactsService>();
            services.AddTransient<GardensService, GardensService>();
            services.AddTransient<LeasingsService, LeasingsService>();
            services.AddTransient<PaymentsService, PaymentsService>();
            services.AddTransient<ScoresService, ScoresService>();
            services.AddTransient<TalksService, TalksService>();
            services.AddTransient<UsersService, UsersService>();
            services.AddTransient<WalletsService, WalletsService>();

            services.Configure<AppSettings>(Configuration.GetSection("authServer"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                Console.WriteLine($"Environment is Development mode");
            }
            else
            {
                app.UseHsts();
                Console.WriteLine($"Environment is Prod mode");
            }

            app.UseRouting();
            app.UseCors("LeMoulinPolicy");
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseMvc(builder =>
            {
                builder.Expand().Filter().OrderBy().Count();
                builder.EnableDependencyInjection();
            });

            app.UseHttpsRedirection();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GardenLink v1");
            });

            using var serviceScope = app.ApplicationServices.CreateScope();
            serviceScope.ServiceProvider.GetService<GardenLinkContext>().Create();
        }
    }
}
