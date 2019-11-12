using FluentValidation.AspNetCore;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Infrastructure.WritableOptions;
using LeagueManager.Persistence.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System.Collections.Generic;
using LeagueManager.Application.AutoMapper;
using AutoMapper;
using System.Reflection;
using LeagueManager.Api.CompetitionApi.AutoMapper;
using LeagueManager.Infrastructure.Configuration;
using LeagueManager.Api.Shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;

namespace LeagueManager.Api.CompetitionApi
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
                    options.Authority = "https://desktop-3pdt884/LeagueManager.IdentityServer";
                    options.Audience = "competitionapi";
                });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateTeamLeagueCommandValidator>())
                .AddApplicationPart(typeof(ConfigurationController).Assembly).AddControllersAsServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Competition API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddDbContext<ILeagueManagerDbContext, LeagueManagerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LeagueManager")));
            services.AddAutoMapper(new Assembly[] {
                typeof(ApplicationProfile).Assembly,
                typeof(CompetitionApiProfile).Assembly
            });

            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IMediator), typeof(CreateTeamLeagueCommandHandler))
                .AddClasses()
                .AsImplementedInterfaces());

            services.AddScoped<IImageFileLoader, ImageFileLoader>();
            services.AddScoped<DbInitializer>();
            services.ConfigureWritable<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.AddScoped<IDbConfigurator, DbConfigurator>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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

            app.UseSwagger(c =>
            {
                c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                {
                    swaggerDoc.Host = httpReq.Host.Value;
                    swaggerDoc.Schemes = new List<string>
                    {
                        "https"
                    };
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Competition API V1");
                c.RoutePrefix = "swagger";
            });
        }
    }
}