using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchSubstitution
{
    public class GetTeamLeagueMatchSubstitutionQueryHandler : IRequestHandler<GetTeamLeagueMatchSubstitutionQuery, SubstitutionDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;
        private readonly ILogger<GetTeamLeagueMatchSubstitutionQueryHandler> logger;

        public GetTeamLeagueMatchSubstitutionQueryHandler(
             ILeagueManagerDbContext context,
            IConfigurationProvider config,
            ILogger<GetTeamLeagueMatchSubstitutionQueryHandler> logger)
                => (this.context, this.config, this.logger) = (context, config, logger);

        public async Task<SubstitutionDto> Handle(GetTeamLeagueMatchSubstitutionQuery request, CancellationToken cancellationToken)
        {
            string methodName = nameof(Handle);
            logger.LogInformation($"{methodName}: Request received");

            logger.LogInformation($"{methodName}: Retrieving substitution");
            var sub = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(l => l.Rounds.SelectMany(r =>
                    r.Matches
                    .Where(m => m.Guid == request.MatchGuid)
                    .SelectMany(m => m.MatchEntries.Where(me => me.Team.Name == request.TeamName)
                        .SelectMany(me => me.Substitutions))
                    )
                )
                .ProjectTo<SubstitutionDto>(config)
                .SingleOrDefaultAsync(l => l.Guid == request.SubstitutionGuid);

            if (sub == null)
            {
                var ex = new SubstitutionNotFoundException(request.SubstitutionGuid);
                logger.LogError(ex, "Substitution '{guid}' was not found", request.SubstitutionGuid);
                throw ex;
            }

            logger.LogInformation($"{methodName}: Returning substitution");
            return sub;
        }
    }
}