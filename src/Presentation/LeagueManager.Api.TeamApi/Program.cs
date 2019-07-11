﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace LeagueManager.Api.TeamApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseIISIntegration()
            .UseStartup<Startup>();
    }
}