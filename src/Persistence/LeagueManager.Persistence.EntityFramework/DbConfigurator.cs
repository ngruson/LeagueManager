using LeagueManager.Application.Config;
using LeagueManager.Application.Interfaces;
using LeagueManager.Infrastructure.Configuration;
using LeagueManager.Infrastructure.WritableOptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace LeagueManager.Persistence.EntityFramework
{
    public class DbConfigurator : IDbConfigurator
    {
        private readonly IWritableOptions<ConnectionStrings> options;
        private readonly IServiceProvider serviceProvider;

        public DbConfigurator(IWritableOptions<ConnectionStrings> options,
            IServiceProvider serviceProvider)
        {
            this.options = options;
            this.serviceProvider = serviceProvider;
        }

        public void Configure(DbConfig dbConfig)
        {
            options.Update(opt =>
            {
                opt.LeagueManager = $"Server={dbConfig.DatabaseServer};Database={dbConfig.DatabaseName};Trusted_Connection=True;Application Name=LeagueManager";
            });

            InitializeDb();
        }

        private async void InitializeDb()
        {
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    var context = scope.ServiceProvider.GetService<ILeagueManagerDbContext>();
                    var concreteContext = (LeagueManagerDbContext)context;
                    concreteContext.Database.Migrate();
                    var initializer = scope.ServiceProvider.GetService<DbInitializer>();
                    await initializer.Initialize(concreteContext);
                }
                catch (Exception ex)
                {
                    var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbConfigurator>>();
                    logger.LogError(ex, "An error occurred while migrating or initializing the database.");
                }
            }
        }
    }
}