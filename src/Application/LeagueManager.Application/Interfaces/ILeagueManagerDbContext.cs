using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ILeagueManagerDbContext
    {
        DbSet<Country> Countries { get; set; }
        DbSet<TeamLeague> TeamLeagues { get; set; }
        DbSet<Team> Teams { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
        DatabaseFacade Database { get; }
    }
}