using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchSubstitution
{
    public class UpdateTeamLeagueMatchSubstitutionCommandHandler : IRequestHandler<UpdateTeamLeagueMatchSubstitutionCommand, SubstitutionDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly ILogger<UpdateTeamLeagueMatchSubstitutionCommandHandler> logger;

        public UpdateTeamLeagueMatchSubstitutionCommandHandler(
            ILeagueManagerDbContext context,
            ILogger<UpdateTeamLeagueMatchSubstitutionCommandHandler> logger
        ) => (this.context, this.logger) = (context, logger);

        public async Task<SubstitutionDto> Handle(UpdateTeamLeagueMatchSubstitutionCommand request, CancellationToken cancellationToken)
        {
            string methodName = nameof(Handle);
            logger.LogInformation($"{methodName}: Request received");

            var sub = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(l => l.Rounds.SelectMany(r =>
                        r.Matches
                        .Where(m => m.Guid == request.MatchGuid)
                        .SelectMany(m => m.MatchEntries.SelectMany(me => me.Substitutions))))
                .Include(l => l.TeamMatchEntry)
                    .ThenInclude(e => e.Team)
                .SingleOrDefaultAsync(l => l.Guid == request.SubstitutionGuid);

            if (sub == null)
            {
                var ex = new SubstitutionNotFoundException(request.SubstitutionGuid);
                logger.LogError(ex, $"{methodName}: Substitution {request.SubstitutionGuid} was not found", request.SubstitutionGuid);
                throw ex;
            }

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
            sub.Minute = request.Minute;
            sub.PlayerOut = playerOut;
            sub.PlayerIn = playerIn;
            logger.LogInformation($"{methodName}: Substitution {sub} was updated", sub);

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"{methodName}: Changes are saved");

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            return config.CreateMapper().Map<SubstitutionDto>(sub);
        }
    }
}