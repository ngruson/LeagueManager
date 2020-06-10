using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatch
{
    public class GetTeamLeagueMatchQueryHandler : IRequestHandler<GetTeamLeagueMatchQuery, TeamMatchDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetTeamLeagueMatchQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TeamMatchDto> Handle(GetTeamLeagueMatchQuery request, CancellationToken cancellationToken)
        {
            var teamLeague = await context.TeamLeagues
                .ProjectTo<TeamLeagueDto>(mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(l => l.Name == request.LeagueName, cancellationToken);

            if (teamLeague != null)
            {
                var match = teamLeague.Rounds.SelectMany(x => x.Matches).SingleOrDefault(m => m.Guid == request.Guid);
                if (match != null)
                {
                    match.MatchEntries = match.MatchEntries
                    .OrderByDescending(me => me.HomeAway.ToString())
                    .ToList();

                    return match;
                }
            }

            return null;
        }
    }
}