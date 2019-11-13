using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Persistence.EntityFramework
{
    public class LeagueManagerDbContext : DbContext, ILeagueManagerDbContext
    {
        public LeagueManagerDbContext(
            DbContextOptions<LeagueManagerDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TeamLeague>()
                .Ignore(e => e.Table);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeagueManagerDbContext).Assembly);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<TeamLeague> TeamLeagues { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}