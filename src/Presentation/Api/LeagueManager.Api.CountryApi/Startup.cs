using AutoMapper;
using LeagueManager.Api.Shared;
using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces;
using LeagueManager.Infrastructure.Configuration;
using LeagueManager.Infrastructure.WritableOptions;
using LeagueManager.Persistence.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace LeagueManager.Api.CountryApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthorization();
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = Configuration["IdentityServer"];
                    options.Audience = "countryapi";
                });

            services.AddMvc(opt => opt.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddApplicationPart(typeof(ConfigurationController).Assembly).AddControllersAsServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Country API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddDbContext<ILeagueManagerDbContext, LeagueManagerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LeagueManager")));

            services.AddAutoMapper(new Assembly[] {
                typeof(MappingProfile).Assembly
            });
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(ILeagueManagerDbContext).Assembly);

            services.AddScoped<IImageFileLoader, ImageFileLoader>();
            services.AddScoped<DbInitializer>();
            services.ConfigureWritable<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddScoped<IDbConfigurator, DbConfigurator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();

            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Country API V1");
                c.RoutePrefix = "swagger";
            });
        }
    }
}