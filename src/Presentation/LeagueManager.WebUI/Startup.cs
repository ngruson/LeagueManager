﻿using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Infrastructure.Api;
using LeagueManager.Infrastructure.HttpHelpers;
using LeagueManager.Infrastructure.WritableOptions;
using LeagueManager.WebUI.AutoMapper;
using LeagueManager.WebUI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IdentityModel.Tokens.Jwt;

namespace LeagueManager.WebUI
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            services.ConfigureWritable<InitSettings>(Configuration.GetSection("InitSettings"));
            services.Configure<ApiSettings>(Configuration.GetSection("ApiSettings"));
            services.AddAutoMapper(typeof(WebUIProfile).Assembly);
            services.AddScoped<IHttpRequestFactory, HttpRequestFactory>();
            services.AddScoped<IHttpRequestBuilder, HttpRequestBuilder>();
            services.AddScoped<ISportApi, SportApi>();
            services.AddScoped<ICountryApi, CountryApi>();
            services.AddScoped<ITeamApi, TeamApi>();
            services.AddScoped<ICompetitionApi, CompetitionApi>();

            services.AddMvc(opt => opt.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
                .AddCookie("Cookies")
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";

                    options.Authority = "https://desktop-3pdt884/LeagueManager.IdentityServer";
                    options.SignInScheme = "Cookies";
                    options.ClientId = "LeagueManager";
                    options.ClientSecret = "secret";
                    options.ResponseType = "code";
                    //options.UsePkce = true;
                    options.SaveTokens = true;
                    options.GetClaimsFromUserInfoEndpoint = true;

                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("offline_access");
                    options.Scope.Add("sportapi");
                    options.Scope.Add("competitionapi");
                    options.Scope.Add("countryapi");
                    options.Scope.Add("teamapi");
                });
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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}