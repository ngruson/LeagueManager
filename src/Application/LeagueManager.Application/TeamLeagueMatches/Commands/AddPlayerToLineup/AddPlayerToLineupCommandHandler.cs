using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Player.Commands.CreatePlayer;
using LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor;
using LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor;
using LeagueManager.Domain.Match;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.AddPlayerToLineup
{
    public class AddPlayerToLineupCommandHandler : IRequestHandler<AddPlayerToLineupCommand, Unit>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMediator mediator;
        private readonly IConfigurationProvider config;
        private readonly ILogger<AddPlayerToLineupCommandHandler> logger;

        public AddPlayerToLineupCommandHandler(ILeagueManagerDbContext context,
            IMediator mediator,
            IConfigurationProvider config,
            ILogger<AddPlayerToLineupCommandHandler> logger) =>
                (this.context, this.mediator, this.config, this.logger) = (context, mediator, config, logger);

        public async Task<Unit> Handle(AddPlayerToLineupCommand request, CancellationToken cancellationToken)
        {
            string methodName = nameof(Handle);
            logger.LogInformation($"{methodName}: Request received");

            logger.LogInformation($"{methodName}: Retrieving player '{request.Player.FullName}'");
            var player = context.Players.AsEnumerable().SingleOrDefault(p => p.FullName == request.Player.FullName);
            if (player == null)
            {
                logger.LogInformation($"{methodName}: Player '{request.Player.FullName}' not found, creating player");
                var createPlayerCommand = config.CreateMapper().Map<CreatePlayerCommand>(request.Player);
                await mediator.Send(createPlayerCommand);
                logger.LogInformation($"{methodName}: Created player '{request.Player.FullName}'");
            }

            var getPlayersRequest = config.CreateMapper().Map<GetPlayersForTeamCompetitorQuery>(request);
            var players = await mediator.Send(getPlayersRequest);
            if (players.Any())
            {
                logger.LogInformation($"{methodName}: Player '{request.Player.FullName}' is not found in team '{request.TeamName}', " +
                    "adding player");
                var command = config.CreateMapper().Map<AddPlayerToTeamCompetitorCommand>(request);
                await mediator.Send(command);
                logger.LogInformation($"{methodName}: Player '{request.Player.FullName}' was added to team '{request.TeamName}'");
            }

            var matchEntry = await Helper.GetMatchEntry(
                nameof(Handle),
                logger,
                context,
                request.LeagueName,
                request.MatchGuid,
                request.TeamName,
                cancellationToken
            );

            var lineup = matchEntry.Lineup.ToList();

            if ((lineup == null) || (lineup.Count == 0))
            {
                var ex = new LineupNotFoundException(request.TeamName);
                logger.LogError(ex, $"Lineup not found for team '{request.TeamName}'");
                throw ex;
            }

            logger.LogInformation($"{methodName}: Adding lineup entry for player {request.Player.FullName}");
            lineup.Add(new TeamMatchEntryLineupEntry
            {
                Number = request.Number,
                Player = player
            });

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"{methodName}: Added lineup entry for player {request.Player.FullName}");
            return Unit.Value;
        }
    }
}