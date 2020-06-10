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

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchGoal
{
    public class GetTeamLeagueMatchGoalQueryHandler : IRequestHandler<GetTeamLeagueMatchGoalQuery, GoalDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;
        private readonly ILogger<GetTeamLeagueMatchGoalQueryHandler> logger;

        public GetTeamLeagueMatchGoalQueryHandler(
             ILeagueManagerDbContext context,
            IConfigurationProvider config,
            ILogger<GetTeamLeagueMatchGoalQueryHandler> logger)
                => (this.context, this.config, this.logger) = (context, config, logger);

        public async Task<GoalDto> Handle(GetTeamLeagueMatchGoalQuery request, CancellationToken cancellationToken)
        {
            string methodName = nameof(Handle);
            logger.LogInformation($"{methodName}: Request received");

            logger.LogInformation($"{methodName}: Retrieving goal");
            var goal = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(l => l.Rounds.SelectMany(r =>
                        r.Matches
                        .Where(m => m.Guid == request.MatchGuid)
                        .SelectMany(m => m.MatchEntries.SelectMany(me => me.Goals))))
                .ProjectTo<GoalDto>(config)
                .SingleOrDefaultAsync(l => l.Guid == request.GoalGuid);

            if (goal == null)
            {
                var ex = new GoalNotFoundException(request.GoalGuid);
                logger.LogError(ex, "Goal '{guid}' was not found", request.GoalGuid);
                throw ex;
            }

            logger.LogInformation($"{methodName}: Returning goal");
            return goal;
        }
    }
}