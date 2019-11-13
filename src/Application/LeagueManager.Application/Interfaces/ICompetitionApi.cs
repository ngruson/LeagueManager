using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.TeamLeagues.Queries.Dto;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;

namespace LeagueManager.Application.Interfaces
{
    public interface ICompetitionApi : IConfigurationApi
    {
        Task<IEnumerable<CompetitionDto>> GetCompetitions(GetCompetitionsQuery query);
        Task<CompetitionDto> GetCompetition(GetCompetitionQuery query);
        Task CreateTeamLeague(CreateTeamLeagueCommand command);
        Task<TeamLeagueTableDto> GetTeamLeagueTable(GetTeamLeagueTableQuery query);
        Task<IEnumerable<TeamLeagueRoundDto>> GetTeamLeagueRounds(GetTeamLeagueRoundsQuery query);
        Task<TeamMatchDto> GetTeamLeagueMatch(GetTeamLeagueMatchQuery query);
        Task<TeamMatchDto> UpdateTeamLeagueMatch(UpdateTeamLeagueMatchCommand command);
        Task<TeamMatchDto> UpdateTeamLeagueMatchScore(UpdateTeamLeagueMatchScoreCommand command);
    }
}