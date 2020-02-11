using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Sports;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ILeagueManagerDbContext
    {
        DbSet<Country> Countries { get; set; }
        DbSet<Domain.Player.Player> Players { get; set; }
        DbSet<TeamLeague> TeamLeagues { get; set; }
        DbSet<Team> Teams { get; set; }
        DbSet<TeamSports> TeamSports { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        bool EnsureCreated();
    }
}