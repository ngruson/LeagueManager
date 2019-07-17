using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Leagues.Commands
{
    public class CreateTeamLeagueCommandHandler : IRequestHandler<CreateTeamLeagueCommand, Unit>
    {
        private readonly ILeagueManagerDbContext context;

        public CreateTeamLeagueCommandHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<Unit> Handle(CreateTeamLeagueCommand request, CancellationToken cancellationToken)
        {
            var competition = await context.Competitions.SingleOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (competition != null)
                throw new CompetitionAlreadyExistsException(request.Name);

            var league = new TeamLeague {
                Name = request.Name,
                Logo = request.Logo
            };

            foreach (var teamName in request.Teams)
            {
                var team = await context.Teams.SingleOrDefaultAsync(t => t.Name == teamName, cancellationToken);
                if (team == null)
                    throw new TeamNotFoundException(teamName);

                league.Teams.Add(new CompetingTeam { Team = team });
            }

            context.Competitions.Add(league);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}