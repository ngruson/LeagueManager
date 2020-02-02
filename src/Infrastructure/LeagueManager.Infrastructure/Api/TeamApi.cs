using LeagueManager.Application.Config;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Queries.GetTeams;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure.Api
{
    public class TeamApi : ITeamApi
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly ApiSettings apiSettings;

        public TeamApi(IHttpRequestFactory httpRequestFactory,
            IOptions<ApiSettings> options)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.apiSettings = options.Value;
        }

        public async Task<bool> Configure(DbConfig dbConfig, string accessToken)
        {
            var response = await httpRequestFactory.Put(
                $"{apiSettings.TeamApiUrl}/configuration",
                dbConfig,
                accessToken
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<TeamDto>> GetTeams()
        {
            var response =  await httpRequestFactory.Get($"{apiSettings.TeamApiUrl}/team");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<TeamDto>>();
            throw new TeamsNotFoundException();
        }
    }
}