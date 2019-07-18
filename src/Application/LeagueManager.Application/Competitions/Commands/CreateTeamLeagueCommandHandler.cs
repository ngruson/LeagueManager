using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Competitor;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Competitions.Commands
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
            var league = await context.TeamLeagues.SingleOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (league != null)
                throw new CompetitionAlreadyExistsException(request.Name);

            Country country = null;
            if (!string.IsNullOrEmpty(request.Country))
                country = await context.Countries.SingleOrDefaultAsync(c => c.Name == request.Country);


            league = new TeamLeague {
                Name = request.Name,
                Country = country,
                Logo = request.Logo
            };

            foreach (var teamName in request.Teams)
            {
                var team = await context.Teams.SingleOrDefaultAsync(t => t.Name == teamName, cancellationToken);
                if (team == null)
                    throw new TeamNotFoundException(teamName);

                league.Teams.Add(new TeamCompetitor { Team = team });
            }

            context.TeamLeagues.Add(league);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}