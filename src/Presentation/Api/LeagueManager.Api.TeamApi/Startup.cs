using FluentValidation.AspNetCore;
using LeagueManager.Api.Shared;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Commands.CreateTeam;
using LeagueManager.Infrastructure.Configuration;
using LeagueManager.Infrastructure.WritableOptions;
using LeagueManager.Persistence.EntityFramework;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeagueManager.Api.TeamApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                builder =>
                {
                    builder.WithOrigins("*");
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtBearerOptions =>
            {
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("MySecretKeyIsSecretSoDoNotTell")),
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(5)
                };
            });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Formatting = Formatting.Indented;
                })
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<CreateTeamCommandValidator>())
                .AddApplicationPart(typeof(ConfigurationController).Assembly).AddControllersAsServices();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Team API", Version = "v1" });
                c.CustomSchemaIds(x => x.FullName);
            });

            services.AddDbContext<ILeagueManagerDbContext, LeagueManagerDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("LeagueManager")));
            
            services.AddScoped<ServiceFactory>(p => p.GetService);
            services.Scan(scan => scan
                .FromAssembliesOf(typeof(IMediator), typeof(CreateTeamCommandHandler))
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

            app.UseCors(MyAllowSpecificOrigins);
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
                    //if (env.IsDevelopment())
                    //    swaggerDoc.Schemes.Add("http");
                });
            });

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "Team API V1");
                c.RoutePrefix = "swagger";
            });
        }
    }
}