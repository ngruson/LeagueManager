using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Application.Teams.Queries.GetTeams
{
    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, IEnumerable<TeamDto>>
    {
        private readonly ILeagueManagerDbContext context;

        public GetTeamsQueryHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
           var teams = await context.Teams
                .Include(team => team.Country)
                .OrderBy(t => t.Name).ToListAsync();
                
            if (teams != null)
                return teams.Select(t => new TeamDto { Name = t.Name, Country = t.Country?.Name });

            return null;
        }
    }
}