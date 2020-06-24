using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Match;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches
{
    public static class Helper
    {
        public static async Task<TeamMatchEntry> GetMatchEntry(string methodName, ILogger logger, ILeagueManagerDbContext context,
            string leagueName, Guid matchGuid, string teamName, CancellationToken cancellationToken)
        {
            logger.LogInformation($"{methodName}: Retrieving match entry for team '{teamName}'");

            var league = await context.TeamLeagues
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.MatchEntries)
                            .ThenInclude(me => me.Team)
                .SingleOrDefaultAsync(x => x.Name == leagueName, cancellationToken);

            if (league == null)
            {
                var ex = new TeamLeagueNotFoundException(leagueName);
                logger.LogError(ex, $"{methodName}: Team league '{leagueName}' not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Team league '{leagueName}' was found");

            var match = league.GetMatch(matchGuid);
            if (match == null)
            {
                var ex = new MatchNotFoundException(matchGuid);
                logger.LogError(ex, $"{methodName}: Match '{matchGuid}' was not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Match '{matchGuid}' was found");

            var matchEntry = match.MatchEntries.SingleOrDefault(me => me.Team.Name == teamName);
            if (matchEntry == null)
            {
                var ex = new MatchEntryNotFoundException(teamName);
                logger.LogError(ex, $"{methodName}: Match entry for team '{teamName}' was not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Match entry for team '{teamName}' was found");

            return matchEntry;
        }

        public static Domain.Player.Player GetPlayer(string methodName, ILogger logger, 
            List<Domain.Player.Player> players, string playerName)
        {
            logger.LogInformation($"{methodName}: Retrieving player '{playerName}'");

            var player = players.AsEnumerable().SingleOrDefault(p => p.FullName == playerName);
            if (player == null)
            {
                var ex = new PlayerNotFoundException(playerName);
                logger.LogError(ex, $"{methodName}: Player '{playerName}' was not found");
                throw ex;
            }
            logger.LogInformation($"{methodName}: Player '{playerName}' was found");
            return player;
        }
    }
}