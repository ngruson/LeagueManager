using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagues.Dto;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;

namespace LeagueManager.Application.Interfaces
{
    public interface ICompetitionApi : IConfigurationApi
    {
        Task<IEnumerable<CompetitionDto>> GetCompetitions(GetCompetitionsQuery query);
        Task<CompetitionDto> GetCompetition(GetCompetitionQuery query);
        Task CreateTeamLeague(CreateTeamLeagueCommand command);
        Task<IEnumerable<TeamCompetitorPlayerDto>> GetPlayersForTeamCompetitor(GetPlayersForTeamCompetitorQuery query);
        Task<TeamLeagueTableDto> GetTeamLeagueTable(GetTeamLeagueTableQuery query);
        Task<IEnumerable<TeamLeagueRoundDto>> GetTeamLeagueRounds(GetTeamLeagueRoundsQuery query);
        Task<TeamMatchDto> GetTeamLeagueMatch(GetTeamLeagueMatchQuery query);
        Task<TeamMatchDto> GetTeamLeagueMatchDetails(GetTeamLeagueMatchDetailsQuery query);
        Task<TeamMatchDto> UpdateTeamLeagueMatch(UpdateTeamLeagueMatchCommand command);
        Task<TeamMatchDto> UpdateTeamLeagueMatchScore(UpdateTeamLeagueMatchScoreCommand command);
        Task<TeamLeagueMatches.Lineup.Dto.LineupEntryDto> GetTeamLeagueMatchLineupEntry(GetTeamLeagueMatchLineupEntryQuery query);
        Task<TeamLeagueMatches.Lineup.Dto.LineupEntryDto> UpdateTeamLeagueMatchLineupEntry(UpdateTeamLeagueMatchLineupEntryCommand command);
    }
}