using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueCompetitors
{
    public class GetTeamLeagueCompetitorsQueryHandler : IRequestHandler<GetTeamLeagueCompetitorsQuery, IEnumerable<CompetitorDto>>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetTeamLeagueCompetitorsQueryHandler(ILeagueManagerDbContext context, IMapper mapper)
            => (this.context, this.mapper) = (context, mapper);

        public async Task<IEnumerable<CompetitorDto>> Handle(GetTeamLeagueCompetitorsQuery request, CancellationToken cancellationToken)
        {
            var teamLeague = await context.TeamLeagues
                .ProjectTo<TeamLeagueDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(x => x.Name == request.LeagueName);

            return teamLeague.Competitors.OrderBy(c => c.TeamName);
        }
    }
}