﻿using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupEntry
{
    public class UpdateTeamLeagueMatchLineupEntryCommandHandler : IRequestHandler<UpdateTeamLeagueMatchLineupEntryCommand, LineupEntryDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;

        public UpdateTeamLeagueMatchLineupEntryCommandHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config)
        {
            this.context = context;
            this.config = config;
        }

        public async Task<LineupEntryDto> Handle(UpdateTeamLeagueMatchLineupEntryCommand request, CancellationToken cancellationToken)
        {
            var lineupEntry = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(l => l.Rounds.SelectMany(r =>
                        r.Matches.Where(m => m.Guid == request.MatchGuid)
                        .SelectMany(m => m.MatchEntries
                            .Where(me => me.Team.Name == request.TeamName)
                            .SelectMany(me => me.Lineup)
                        )
                    )
                )
                .Include(l =>l.TeamMatchEntry)
                    .ThenInclude(e => e.Team)
                .SingleOrDefaultAsync(l => l.Guid == request.LineupEntryGuid);

            if (lineupEntry == null)
                throw new LineupEntryNotFoundException(request.LineupEntryGuid);

            var player = context.Players.AsEnumerable().SingleOrDefault(p => p.FullName == request.PlayerName);
            lineupEntry.Number = request.PlayerNumber;
            lineupEntry.Player = player ?? throw new PlayerNotFoundException(request.PlayerName);
            await context.SaveChangesAsync(cancellationToken);

            return config.CreateMapper().Map<LineupEntryDto>(lineupEntry);
        }
    }
}