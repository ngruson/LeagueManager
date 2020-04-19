using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Goals;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchGoal
{
    public class UpdateTeamLeagueMatchGoalCommandHandler : IRequestHandler<UpdateTeamLeagueMatchGoalCommand, GoalDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;
        private readonly ILogger<UpdateTeamLeagueMatchGoalCommandHandler> logger;

        public UpdateTeamLeagueMatchGoalCommandHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config,
            ILogger<UpdateTeamLeagueMatchGoalCommandHandler> logger
        ) => (this.context, this.config, this.logger) = (context, config, logger);

        public async Task<GoalDto> Handle(UpdateTeamLeagueMatchGoalCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("Request received");

            var goal = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(l => l.Rounds.SelectMany(r =>
                        r.Matches
                        .Where(m => m.Guid == request.MatchGuid)
                        .SelectMany(m => m.MatchEntries.SelectMany(me => me.Goals))))
                .Include(l => l.TeamMatchEntry)
                    .ThenInclude(e => e.Team)
                .SingleOrDefaultAsync(l => l.Guid == request.GoalGuid);

            var player = await context.Players.SingleOrDefaultAsync(p => p.FullName == request.PlayerName);
            if (player == null)
            {
                var ex = new PlayerNotFoundException(request.PlayerName);
                logger.LogError(ex, "Player {playerName} was not found", request.PlayerName);
                throw ex;
            }
            logger.LogInformation("Player {playerName} was found", request.PlayerName);

            if (goal == null)
            {
                var ex = new GoalNotFoundException(request.GoalGuid);
                logger.LogError(ex, "Goal {guid} was not found", request.GoalGuid);
                throw ex;
            }

            goal.Minute = request.Minute;
            goal.Player = player;
            logger.LogInformation("Goal {goal} was updated", goal);

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Changes are saved");

            //return await mediator.send GetTeamLeagueMatchGoalQuery
            return config.CreateMapper().Map<GoalDto>(goal);
        }
    }
}