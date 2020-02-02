using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Dto;
using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competition;
using LeagueManager.Domain.Sports;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Commands
{
    public class CreateTeamLeagueCommandHandler : IRequestHandler<CreateTeamLeagueCommand, TeamLeagueDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public CreateTeamLeagueCommandHandler(ILeagueManagerDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TeamLeagueDto> Handle(CreateTeamLeagueCommand request, CancellationToken cancellationToken)
        {
            var league = await context.TeamLeagues.SingleOrDefaultAsync(x => x.Name == request.Name, cancellationToken);
            if (league != null)
                throw new CompetitionAlreadyExistsException(request.Name);

            var sports = await context.TeamSports
                .Include(t => t.Options)
                .SingleOrDefaultAsync(ts => ts.Name == request.Sports);
                
            if (sports == null)
                throw new SportsNotFoundException(request.Sports);

            Country country = null;
            if (!string.IsNullOrEmpty(request.Country))
            {
                country = await context.Countries.SingleOrDefaultAsync(c => c.Name == request.Country);
                if (country == null)
                    throw new CountryNotFoundException(request.Country);
            }

            league = new TeamLeague {
                Sports = sports,
                Name = request.Name,
                Country = country,
                Logo = request.Logo
            };

            foreach (var teamName in request.Teams)
            {
                var team = await context.Teams.SingleOrDefaultAsync(t => t.Name == teamName, cancellationToken);
                if (team == null)
                    throw new TeamNotFoundException(teamName);

                league.Competitors.Add(new Domain.Competitor.TeamCompetitor { Team = team });
            }

            league.CreateRounds();

            context.TeamLeagues.Add(league);
            await context.SaveChangesAsync(cancellationToken);

            return mapper.Map<TeamLeagueDto>(league);
        }
    }
}