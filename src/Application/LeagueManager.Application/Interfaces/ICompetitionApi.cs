using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagues.Commands.CreateTeamLeague;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeague;
using System;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution;

namespace LeagueManager.Application.Interfaces
{
    public interface ICompetitionApi : IConfigurationApi
    {
        Task<IEnumerable<Competitions.Queries.GetCompetitions.CompetitionDto>> GetCompetitions(GetCompetitionsQuery query);
        Task<Competitions.Queries.GetCompetition.CompetitionDto> GetCompetition(GetCompetitionQuery query);
        Task CreateTeamLeague(CreateTeamLeagueCommand command);
        Task<IEnumerable<CompetitorDto>> GetCompetitors(GetTeamLeagueCompetitorsQuery query);
        Task<IEnumerable<CompetitorPlayerDto>> GetPlayersForTeamCompetitor(GetPlayersForTeamCompetitorQuery query);
        Task<GetTeamLeagueVm> GetTeamLeague(string leagueName);
        Task<GetTeamLeagueRoundsVm> GetTeamLeagueRounds(GetTeamLeagueRoundsQuery query);
        Task<GetTeamLeagueTableVm> GetTeamLeagueTable(GetTeamLeagueTableQuery query);
        Task<TeamLeagueMatches.Queries.GetTeamLeagueMatch.TeamMatchDto> GetTeamLeagueMatch(GetTeamLeagueMatchQuery query);
        Task<TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails.TeamMatchDto> GetTeamLeagueMatchDetails(GetTeamLeagueMatchDetailsQuery query);
        Task<TeamLeagueMatches.Commands.UpdateTeamLeagueMatch.TeamMatchDto> UpdateTeamLeagueMatch(UpdateTeamLeagueMatchCommand command);
        Task<TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore.TeamMatchDto> UpdateTeamLeagueMatchScore(string leagueName, Guid guid, UpdateTeamLeagueMatchScoreDto dto);
        Task<TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry.LineupEntryDto> GetTeamLeagueMatchLineupEntry(GetTeamLeagueMatchLineupEntryQuery query);
        Task<TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry.LineupEntryDto> UpdateTeamLeagueMatchLineupEntry(UpdateTeamLeagueMatchLineupEntryCommand command);
        Task<MatchEventsDto> GetTeamLeagueMatchEvents(GetTeamLeagueMatchEventsQuery query);
        Task<TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal.GoalDto> GetTeamLeagueMatchGoal(GetTeamLeagueMatchGoalQuery query);
        Task<TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal.GoalDto> UpdateTeamLeagueMatchGoal(UpdateTeamLeagueMatchGoalCommand command);
        Task<TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution.SubstitutionDto> GetTeamLeagueMatchSubstitution(GetTeamLeagueMatchSubstitutionQuery query);
        Task<TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution.SubstitutionDto> UpdateTeamLeagueMatchSubstitution(
            string leagueName, Guid matchGuid, string teamName, Guid substitutionGuid,            
            UpdateTeamLeagueMatchSubstitutionDto dto);
    }
}