﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.Config;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails;
using LeagueManager.Application.TeamLeagues.Dto;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Commands.UpdateTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents;
using LeagueManager.Application.TeamLeagueMatches.Goals;
using LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal;
using LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal;

namespace LeagueManager.Infrastructure.Api
{
    public class CompetitionApi : ICompetitionApi
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly ApiSettings apiSettings;
        private readonly string teamLeagueApiUrl;

        public CompetitionApi(IHttpRequestFactory httpRequestFactory,
            IOptions<ApiSettings> options)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.apiSettings = options.Value;
            this.teamLeagueApiUrl = $"{apiSettings.CompetitionApiUrl}/Competition/teamleague"; 
        }

        public async Task<bool> Configure(DbConfig dbConfig, string accessToken)
        {
            var response = await httpRequestFactory.Put(
                $"{apiSettings.CompetitionApiUrl}/configuration",
                dbConfig,
                accessToken
            );

            return response.IsSuccessStatusCode;
        }

        public async Task CreateTeamLeague(CreateTeamLeagueCommand command)
        {
            var response = await httpRequestFactory.Post(teamLeagueApiUrl, command);
            if (!response.IsSuccessStatusCode)
                throw new CreateTeamLeagueException(command.Name);
        }

        public async Task<IEnumerable<CompetitionDto>> GetCompetitions(GetCompetitionsQuery query)
        {
            var response = await httpRequestFactory.Get($"{apiSettings.CompetitionApiUrl}/competition");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<CompetitionDto>>();
            return new List<CompetitionDto>();
        }

        public async Task<CompetitionDto> GetCompetition(GetCompetitionQuery query)
        {
            var response = await httpRequestFactory.Get($"{apiSettings.CompetitionApiUrl}/competition/{query.Name}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<CompetitionDto>();
            return new CompetitionDto();
        }

        public async Task<IEnumerable<TeamCompetitorPlayerDto>> GetPlayersForTeamCompetitor(GetPlayersForTeamCompetitorQuery query)
        {
            var response = await httpRequestFactory.Get($"{apiSettings.TeamLeagueApiUrl}/{query.LeagueName}/competitor/{query.TeamName}/players");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<TeamCompetitorPlayerDto>>();
            return null;
        }

        public async Task<TeamLeagueTableDto> GetTeamLeagueTable(GetTeamLeagueTableQuery query)
        {
            string requestUri = $"{teamLeagueApiUrl}/{query.LeagueName}/table";
            var response = await httpRequestFactory.Get(requestUri);
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<TeamLeagueTableDto>();
            return null;
        }

        public async Task<IEnumerable<TeamLeagueRoundDto>> GetTeamLeagueRounds(GetTeamLeagueRoundsQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/rounds");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<TeamLeagueRoundDto>>();
            return null;
        }

        public async Task<TeamMatchDto> GetTeamLeagueMatch(GetTeamLeagueMatchQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/match/{query.Guid}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<TeamMatchDto>();
            return null;
        }

        public async Task<TeamMatchDto> GetTeamLeagueMatchDetails(GetTeamLeagueMatchDetailsQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/match/details/{query.Guid}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<TeamMatchDto>();
            return null;
        }

        public async Task<TeamMatchDto> UpdateTeamLeagueMatch(UpdateTeamLeagueMatchCommand command)
        {
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{command.LeagueName}/match/{command.Guid}",
                command
            );

            if (response.IsSuccessStatusCode)
                return response.ContentAsType<TeamMatchDto>();
            return null;
        }

        public async Task<TeamMatchDto> UpdateTeamLeagueMatchScore(UpdateTeamLeagueMatchScoreCommand command)
        {
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{command.LeagueName}/match/{command.Guid}/score",
                command
            );

            if (response.IsSuccessStatusCode)
                return response.ContentAsType<TeamMatchDto>();
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Lineup.Dto.LineupEntryDto> GetTeamLeagueMatchLineupEntry(GetTeamLeagueMatchLineupEntryQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/match/{query.MatchGuid}/lineup/{query.LineupEntryGuid}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<Application.TeamLeagueMatches.Lineup.Dto.LineupEntryDto>();
            return null;
        }

        public async Task<Application.TeamLeagueMatches.Lineup.Dto.LineupEntryDto> UpdateTeamLeagueMatchLineupEntry(UpdateTeamLeagueMatchLineupEntryCommand command)
        {
            var dto = new UpdateLineupEntryDto
            {
                PlayerNumber = command.PlayerNumber,
                PlayerName = command.PlayerName
            };
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{command.LeagueName}/match/{command.MatchGuid}/{command.TeamName}/lineup/{command.LineupEntryGuid}",
                dto
            );

            if (response.IsSuccessStatusCode)
                return response.ContentAsType<Application.TeamLeagueMatches.Lineup.Dto.LineupEntryDto>();
            return null;
        }

        public async Task<MatchEventsDto> GetTeamLeagueMatchEvents(GetTeamLeagueMatchEventsQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/match/{query.MatchGuid}/{query.TeamName}/events");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<MatchEventsDto>();
            return null;
        }

        public async Task<GoalDto> GetTeamLeagueMatchGoal(GetTeamLeagueMatchGoalQuery query)
        {
            var response = await httpRequestFactory.Get($"{teamLeagueApiUrl}/{query.LeagueName}/match/{query.MatchGuid}/goal/{query.GoalGuid}");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<GoalDto>();
            return null;
        }

        public async Task<GoalDto> UpdateTeamLeagueMatchGoal(UpdateTeamLeagueMatchGoalCommand command)
        {
            var dto = new AddTeamLeagueMatchGoalDto
            {
                Minute = command.Minute,
                PlayerName = command.PlayerName
            };
            var response = await httpRequestFactory.Put(
                $"{teamLeagueApiUrl}/{command.LeagueName}/match/{command.MatchGuid}/team/{command.TeamName}/goal/{command.GoalGuid}",
                dto
            );

            if (response.IsSuccessStatusCode)
                return response.ContentAsType<GoalDto>();
            return null;
        }
    }
}