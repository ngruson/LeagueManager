using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Sports.Queries.GetTeamSports
{
    public class GetTeamSportsQueryHandler : IRequestHandler<GetTeamSportsQuery, IEnumerable<TeamSportDto>>
    {
        private readonly ILeagueManagerDbContext context;

        public GetTeamSportsQueryHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TeamSportDto>> Handle(GetTeamSportsQuery request, CancellationToken cancellationToken)
        {
            var teamSports = await context.TeamSports
                .OrderBy(t => t.Name)
                .Include(t => t.Options)
                .ToListAsync();

            if (teamSports != null)
                return teamSports.Select(t => new TeamSportDto { 
                    Name = t.Name,
                    Options = new TeamSportOptionsDto
                    {
                        AmountOfPlayers = t.Options.AmountOfPlayers
                    }    
                });

            return null;
        }
    }
}