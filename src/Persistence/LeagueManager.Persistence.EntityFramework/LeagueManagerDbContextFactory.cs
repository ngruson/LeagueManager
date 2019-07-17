using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Persistence.EntityFramework
{
    public class LeagueManagerDbContextFactory : DesignTimeDbContextFactoryBase<LeagueManagerDbContext>
    {
        protected override LeagueManagerDbContext CreateNewInstance(DbContextOptions<LeagueManagerDbContext> options)
        {
            return new LeagueManagerDbContext(options);
        }
    }
}