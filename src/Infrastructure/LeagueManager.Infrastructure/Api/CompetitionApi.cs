using System.Collections.Generic;
using System.Threading.Tasks;
using LeagueManager.Application.Competitions.Commands;
using LeagueManager.Application.Competitions.Queries.GetCompetitions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Infrastructure.Exceptions;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;

namespace LeagueManager.Infrastructure.Api
{
    public class CompetitionApi : ICompetitionApi
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly ApiSettings apiSettings;

        public CompetitionApi(IHttpRequestFactory httpRequestFactory,
            IOptions<ApiSettings> options)
        {
            this.httpRequestFactory = httpRequestFactory;
            this.apiSettings = options.Value;
        }

        public async Task CreateTeamLeague(CreateTeamLeagueCommand command)
        {
            var response = await httpRequestFactory.Post(apiSettings.TeamLeagueApiUrl, command);
            if (!response.IsSuccessStatusCode)
                throw new CreateTeamLeagueException(command.Name);
        }

        public async Task<IEnumerable<CompetitionDto>> GetCompetitions(GetCompetitionsQuery query)
        {
            var response = await httpRequestFactory.Get(apiSettings.CompetitionApiUrl);
            if (response.IsSuccessStatusCode)
                return response.ContentAsType<IEnumerable<CompetitionDto>>();
            return new List<CompetitionDto>();
        }
    }
}