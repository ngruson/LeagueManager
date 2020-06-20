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

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddTeamLeagueMatchSubstitution
{
    public class AddTeamLeagueMatchSubstitutionCommandHandler : IRequestHandler<AddTeamLeagueMatchSubstitutionCommand, SubstitutionDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;
        private readonly ILogger<AddTeamLeagueMatchSubstitutionCommandHandler> logger;

        public AddTeamLeagueMatchSubstitutionCommandHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config,
            ILogger<AddTeamLeagueMatchSubstitutionCommandHandler> logger
        ) => (this.context, this.config, this.logger) = (context, config, logger);

        public async Task<SubstitutionDto> Handle(AddTeamLeagueMatchSubstitutionCommand request, CancellationToken cancellationToken)
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

            var players = context.Players.ToList();
            Domain.Player.Player GetPlayer(string playerName)
            {
                var player = players.AsEnumerable().SingleOrDefault(p => p.FullName == playerName);
                if (player == null)
                {
                    var ex = new PlayerNotFoundException(playerName);
                    logger.LogError(ex, $"{methodName}: Player {playerName} was not found");
                    throw ex;
                }
                logger.LogInformation($"{methodName}: Player {playerName} was found");
                return player;
            }

            var playerOut = GetPlayer(request.PlayerOut);
            var playerIn = GetPlayer(request.PlayerIn);

            var subs = matchEntry.Substitutions.ToList();
            var sub = new TeamMatchEntrySubstitution
            {
                Guid = Guid.NewGuid(),
                Minute = request.Minute,
                PlayerOut = playerOut,
                PlayerIn = playerIn
            };

            subs.Add(sub);
            matchEntry.Substitutions = subs;
            logger.LogInformation($"{methodName}: Substitution {sub} was added");

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"{methodName}: Changes are saved");

            return config.CreateMapper().Map<SubstitutionDto>(sub);
        }
    }
}