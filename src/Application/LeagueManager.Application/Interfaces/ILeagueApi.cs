using LeagueManager.Application.Leagues.Commands;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ILeagueApi
    {
        Task CreateTeamLeague(CreateTeamLeagueCommand command);

    }
}