using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
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
            modelBuilder.Entity<TeamLeague>();
            modelBuilder.Entity<TeamMatch>();
            modelBuilder.Entity<IntegerScore>();
            modelBuilder.Entity<Team>();

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(LeagueManagerDbContext).Assembly);
        }

        public DbSet<Country> Countries { get; set; }
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Team> Teams { get; set; }
    }
}