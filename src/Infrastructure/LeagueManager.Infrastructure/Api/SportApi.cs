using LeagueManager.Application.Config;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Sports.Queries.GetTeamSports;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeagueManager.Infrastructure.Api
{
    public class SportApi : ISportApi
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly ApiSettings apiSettings;

        public SportApi(IHttpRequestFactory httpRequestFactory,
            IOptions<ApiSettings> options)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.apiSettings = options.Value;
        }

        public async Task<bool> Configure(DbConfig dbConfig, string accessToken)
        {
            var response = await httpRequestFactory.Put(
                $"{apiSettings.SportApiUrl}/configuration",
                dbConfig,
                accessToken
            );

            return response.IsSuccessStatusCode;
        }

        public async Task<IEnumerable<TeamSportDto>> GetTeamSports()
        {
            var response = await httpRequestFactory.Get($"{apiSettings.SportApiUrl}/teamsports");
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<TeamSportDto>>();
            return null;
        }
    }
}