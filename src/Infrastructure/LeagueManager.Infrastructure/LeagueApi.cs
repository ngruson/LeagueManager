using System.Threading.Tasks;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Leagues.Commands;
using LeagueManager.Infrastructure.Exceptions;
using LeagueManager.Infrastructure.HttpHelpers;
using Microsoft.Extensions.Options;

namespace LeagueManager.Infrastructure
{
    public class LeagueApi : ILeagueApi
    {
        private readonly IHttpRequestFactory httpRequestFactory;
        private readonly ApiSettings apiSettings;

        public LeagueApi(IHttpRequestFactory httpRequestFactory,
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
    }
}