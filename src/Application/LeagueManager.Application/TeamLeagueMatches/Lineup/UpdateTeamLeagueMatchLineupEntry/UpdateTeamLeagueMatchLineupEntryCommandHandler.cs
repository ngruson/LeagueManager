using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatchLineupPlayer
{
    public class UpdateTeamLeagueMatchLineupEntryCommandHandler : IRequestHandler<UpdateTeamLeagueMatchLineupEntryCommand, LineupEntryDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMediator mediator;
        private readonly IConfigurationProvider config;

        public UpdateTeamLeagueMatchLineupEntryCommandHandler(
            ILeagueManagerDbContext context,
            IMediator mediator,
            IConfigurationProvider config)
        {
            this.context = context;
            this.mediator = mediator;
            this.config = config;
        }

        public async Task<LineupEntryDto> Handle(UpdateTeamLeagueMatchLineupEntryCommand request, CancellationToken cancellationToken)
        {
            var lineupEntry = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .Select(l => l.Rounds.SelectMany(r =>
                        r.Matches.Where(m => m.Guid == request.MatchGuid)
                    ).FirstOrDefault())
                .SelectMany(m => m.MatchEntries.SelectMany(me => me.Lineup))
                .Include(l =>l.TeamMatchEntry)
                    .ThenInclude(e => e.Team)
                .SingleOrDefaultAsync(l => l.Guid == request.LineupEntryGuid);

            if (lineupEntry == null)
                throw new LineupEntryNotFoundException(request.LineupEntryGuid);

            var player = context.Players.SingleOrDefault(p => p.FullName == request.PlayerName);
            lineupEntry.Number = request.PlayerNumber;
            lineupEntry.Player = player ?? throw new PlayerNotFoundException(request.PlayerName);
            await context.SaveChangesAsync(cancellationToken);

            return config.CreateMapper().Map<LineupEntryDto>(lineupEntry);
        }
    }
}