using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueMatch
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
                .Include(t => t.Country)
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team)
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.MatchEntries)
                .SingleOrDefaultAsync(l => l.Name == request.LeagueName, cancellationToken);

            if (teamLeague != null)
            {
                var match = teamLeague.Rounds.SelectMany(x => x.Matches).SingleOrDefault(m => m.Guid == request.Guid);
                if (match != null)
                {
                    match.MatchEntries.ForEach(me =>
                    {
                        var competitor = teamLeague.Competitors.SingleOrDefault(c => c.Team.Id == me.TeamId);
                        if (competitor != null && competitor.Team != null)
                            me.Team = competitor.Team;
                    });

                    match.MatchEntries = match.MatchEntries
                        .OrderByDescending(me => me.HomeAway.ToString())
                        .ToList();

                    return mapper.Map<TeamMatchDto>(match);
                }
            }

            return null;
        }
    }
}