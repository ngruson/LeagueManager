using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class GetTeamLeagueMatchEventsQueryHandler : IRequestHandler<GetTeamLeagueMatchEventsQuery, MatchEventsVm>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;
        private readonly ILogger<GetTeamLeagueMatchEventsQueryHandler> logger;

        public GetTeamLeagueMatchEventsQueryHandler(
                ILeagueManagerDbContext context,
                IConfigurationProvider config,
                ILogger<GetTeamLeagueMatchEventsQueryHandler> logger
            ) => (this.context, this.config, this.logger) = (context, config, logger);

        public async Task<MatchEventsVm> Handle(GetTeamLeagueMatchEventsQuery request, CancellationToken cancellationToken)
        {
            string methodName = nameof(Handle);
            logger.LogInformation($"{methodName}: Request received");

            logger.LogInformation($"{methodName}: Retrieving match events");
            var goals = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(t => t.Rounds.SelectMany(r => r.Matches
                    .Where(m => m.Guid == request.MatchGuid)
                .SelectMany(m => m.MatchEntries.Where(me => me.Team.Name == request.TeamName))
                .SelectMany(me => me.Goals)
                ))
                .ProjectTo<GoalVm>(config)
                .ToListAsync();
            
            logger.LogInformation($"{methodName}: Returning match events");
            return new MatchEventsVm { Goals = goals };
        }
    }
}