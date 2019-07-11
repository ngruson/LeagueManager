using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using MediatR;

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
            List<Team> teams = null;
            await Task.Run(() =>
            {
                teams = context.Teams.OrderBy(t => t.Name).ToList();
            });
                
            if (teams != null)
                return teams.Select(t => new TeamDto { Name = t.Name });

            return null;
        }
    }
}