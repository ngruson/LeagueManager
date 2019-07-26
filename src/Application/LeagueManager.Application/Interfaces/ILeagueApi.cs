using LeagueManager.Application.Competitions.Commands;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Competitions.Queries.GetTeamLeague;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Application.Interfaces
{
    public interface ICompetitionApi
    {
        Task<IEnumerable<CompetitionDto>> GetCompetitions(GetCompetitionsQuery query);
        Task CreateTeamLeague(CreateTeamLeagueCommand command);
        Task<TeamLeagueDto> ViewTeamLeague(GetTeamLeagueQuery query);
    }
}