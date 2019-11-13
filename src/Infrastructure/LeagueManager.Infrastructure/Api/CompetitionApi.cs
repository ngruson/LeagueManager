using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Application.TeamLeagues.Commands;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.Dto;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using LeagueManager.Application.Competitions.Queries.GetCompetition;
using LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch;
using LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchScore;
using LeagueManager.Application.Config;
using LeagueManager.Application.Exceptions;

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
    }
}