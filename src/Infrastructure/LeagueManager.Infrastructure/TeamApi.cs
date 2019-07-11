using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Teams.Queries.GetTeams;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure
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

        public async Task<IEnumerable<TeamDto>> GetTeams()
        {
            var response =  await httpRequestFactory.Get(apiSettings.TeamApiUrl);
            return response.ContentAsType<IEnumerable<TeamDto>>();
        }
    }
}