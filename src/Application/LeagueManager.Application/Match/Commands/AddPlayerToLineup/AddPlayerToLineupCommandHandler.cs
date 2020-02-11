using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Match.Commands.AddPlayerToLineup
{
    public class AddPlayerToLineupCommandHandler : IRequestHandler<AddPlayerToLineupCommand, Unit>
    {
        private readonly ILeagueManagerDbContext context;

        public AddPlayerToLineupCommandHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(AddPlayerToLineupCommand request, CancellationToken cancellationToken)
        {
            var lineup = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .Select(l => l.Rounds.SelectMany(r =>
                        r.Matches.Where(m => m.Guid == request.Guid)
                    ).FirstOrDefault())
                .SelectMany(m => m.MatchEntries.SelectMany(me => me.Lineup))
                .Where(l => l.TeamMatchEntry.Team.Name == request.Team)
                .ToListAsync();

            if ((lineup == null) || (lineup.Count == 0))
                throw new MatchEntryNotFoundException(request.Team);

            var player = await context.Players.SingleOrDefaultAsync(p => p.FullName == request.Player);
            if (player == null)
                throw new PlayerNotFoundException(request.Player);

            lineup.Add(new Domain.Match.TeamMatchEntryLineupEntry
            {
                Number = request.Number,
                Player = player
            });

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}