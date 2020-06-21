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
            var matchEntry = await Helper.GetMatchEntry(
                nameof(Handle),
                logger,
                context,
                request.LeagueName,
                request.MatchGuid,
                request.TeamName,
                cancellationToken
            );

            var players = context.Players.ToList();
            var playerOut = Helper.GetPlayer(nameof(Handle), logger, players, request.PlayerOut);
            var playerIn = Helper.GetPlayer(nameof(Handle), logger, players, request.PlayerIn);

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
            logger.LogInformation($"{nameof(Handle)}: Substitution {sub} was added");

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation($"{nameof(Handle)}: Changes are saved");

            return config.CreateMapper().Map<SubstitutionDto>(sub);
        }
    }
}