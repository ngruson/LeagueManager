using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Persistence.EntityFramework
{
    public class LeagueManagerDbContextFactory : DesignTimeDbContextFactoryBase<LeagueManagerDbContext>
    {
        private readonly IImageFileLoader imageFileLoader;

        public LeagueManagerDbContextFactory(IImageFileLoader imageFileLoader)
        {
            this.imageFileLoader = imageFileLoader;
        }

        protected override LeagueManagerDbContext CreateNewInstance(DbContextOptions<LeagueManagerDbContext> options)
        {
            return new LeagueManagerDbContext(options, imageFileLoader);
        }
    }
}