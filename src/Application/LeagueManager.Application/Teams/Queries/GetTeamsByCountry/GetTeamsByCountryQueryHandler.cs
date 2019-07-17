using LeagueManager.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Application.Teams.Queries.GetTeamsByCountry
{
    public class GetTeamsByCountryQueryHandler : IRequestHandler<GetTeamsByCountryQuery, IEnumerable<TeamDto>>
    {
        private readonly ILeagueManagerDbContext context;

        public GetTeamsByCountryQueryHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<TeamDto>> Handle(GetTeamsByCountryQuery request, CancellationToken cancellationToken)
        {
            var teams = await context.Teams
                .Include(team => team.Country)
                .Where(t => t.Country != null && t.Country.Name == request.Country)
                .OrderBy(t => t.Name).ToListAsync();

            if (teams != null)
                return teams.Select(t => new TeamDto { Name = t.Name, Country = t.Country?.Name });

            return null;
        }
    }
}