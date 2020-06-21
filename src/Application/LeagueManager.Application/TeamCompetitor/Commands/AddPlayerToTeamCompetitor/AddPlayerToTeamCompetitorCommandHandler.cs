using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Competitor;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Application.TeamCompetitor.Commands.AddPlayerToTeamCompetitor
{
    public class AddPlayerToTeamCompetitorCommandHandler : IRequestHandler<AddPlayerToTeamCompetitorCommand>
    {
        private readonly ILeagueManagerDbContext context;

        public AddPlayerToTeamCompetitorCommandHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(AddPlayerToTeamCompetitorCommand request, CancellationToken cancellationToken)
        {
            var player = context.Players.AsEnumerable().SingleOrDefault(p => p.FullName == request.PlayerName);
            if (player == null)
                throw new PlayerNotFoundException(request.PlayerName);

            var teamLeague = await context.TeamLeagues
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team)
                .SingleOrDefaultAsync(t => t.Name == request.LeagueName);
                
            if (teamLeague == null)
                throw new TeamLeagueNotFoundException(request.LeagueName);

            var team = teamLeague.Competitors
                .SingleOrDefault(t => t.Team.Name == request.TeamName);
            if (team == null)
                throw new TeamNotFoundException(request.TeamName);

            if (!team.Players.Any(p => p.Player.FullName == request.PlayerName))
            {
                team.Players.Add(new TeamCompetitorPlayer
                {
                    Number = request.PlayerNumber,
                    Player = player
                });
                await context.SaveChangesAsync(cancellationToken);
            }

            return Unit.Value;
        }
    }
}