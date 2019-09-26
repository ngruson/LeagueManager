using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Match;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Application.TeamLeagueMatches.Commands
{
    public class UpdateTeamLeagueMatchCommandHandler : IRequestHandler<UpdateTeamLeagueMatchCommand, Unit>
    {
        private readonly ILeagueManagerDbContext context;

        public UpdateTeamLeagueMatchCommandHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(UpdateTeamLeagueMatchCommand request, CancellationToken cancellationToken)
        {
            var league = await context.TeamLeagues
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.MatchEntries)
                .SingleOrDefaultAsync(x => x.Name == request.LeagueName, cancellationToken);

            var match = league.GetMatch(request.Guid);
            if (match == null)
                throw new MatchNotFoundException(request.Guid);

            var homeMatchEntry = match.MatchEntries.SingleOrDefault(
                me => me.HomeAway == HomeAway.Home);
            var homeTeam = await context.Teams.SingleOrDefaultAsync(t => t.Name == request.HomeTeam, cancellationToken);
            homeMatchEntry.Team = homeTeam ?? throw new TeamNotFoundException(request.HomeTeam);

            var awayMatchEntry = match.MatchEntries.SingleOrDefault(
                me => me.HomeAway == HomeAway.Away);
            var awayTeam = await context.Teams.SingleOrDefaultAsync(t => t.Name == request.AwayTeam, cancellationToken);
            awayMatchEntry.Team = awayTeam ?? throw new TeamNotFoundException(request.AwayTeam);

            match.StartTime = request.StartTime;

            await context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}