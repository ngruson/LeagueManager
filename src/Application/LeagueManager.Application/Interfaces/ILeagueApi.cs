using LeagueManager.Application.Competitions.Commands;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ICompetitionApi
    {
        Task<IEnumerable<CompetitionDto>> GetCompetitions(GetCompetitionsQuery query);
        Task CreateTeamLeague(CreateTeamLeagueCommand command);

    }
}