using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Match;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchGoal
{
    public class AddTeamLeagueMatchGoalCommandHandler : IRequestHandler<AddTeamLeagueMatchGoalCommand, GoalDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;
        private readonly ILogger<AddTeamLeagueMatchGoalCommandHandler> logger;

        public AddTeamLeagueMatchGoalCommandHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config,
            ILogger<AddTeamLeagueMatchGoalCommandHandler> logger
        ) => (this.context, this.config, this.logger) = (context, config, logger);

        public async Task<GoalDto> Handle(AddTeamLeagueMatchGoalCommand request, CancellationToken cancellationToken)
        {
            string methodName = nameof(Handle);
            logger.LogInformation($"{methodName}: Request received");

            var league = await context.TeamLeagues
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.MatchEntries)
                            .ThenInclude(me => me.Team)
                .SingleOrDefaultAsync(x => x.Name == request.LeagueName, cancellationToken);

            if (league == null)
            {
                var ex = new TeamLeagueNotFoundException(request.LeagueName);
                logger.LogError(ex, $"{methodName}: Team league {request.LeagueName} not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Team league {request.LeagueName} was found");

            var match = league.GetMatch(request.MatchGuid);
            if (match == null)
            {
                var ex = new MatchNotFoundException(request.MatchGuid);
                logger.LogError(ex, $"{methodName}: Match {request.MatchGuid} was not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Match {request.MatchGuid} was found");

            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == request.TeamName);
            if (matchEntry == null)
            {
                var ex = new MatchEntryNotFoundException(request.TeamName);
                logger.LogError(ex, $"{methodName}: Match entry for team {request.TeamName} was not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Match entry for team {request.TeamName} was found");

            var player = context.Players.AsEnumerable().SingleOrDefault(p => p.FullName == request.PlayerName);
            if (player == null)
            {
                var ex = new PlayerNotFoundException(request.PlayerName);
                logger.LogError(ex, $"{methodName}: Player {request.PlayerName} was not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Player {request.PlayerName} was found");

            var goals = matchEntry.Goals.ToList();
            var goal = new TeamMatchEntryGoal
            {
                Guid = Guid.NewGuid(),
                Minute = request.Minute,
                Player = player
            };

            goals.Add(goal);
            matchEntry.Goals = goals;
            logger.LogInformation($"{methodName}: Goal {goal} was added");

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"{methodName}: Changes are saved");

            return config.CreateMapper().Map<GoalDto>(goal);
        }
    }
}