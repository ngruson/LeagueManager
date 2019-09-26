using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueRounds
{
    public class GetTeamLeagueRoundsQueryHandler : IRequestHandler<GetTeamLeagueRoundsQuery, IEnumerable<TeamLeagueRoundDto>>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetTeamLeagueRoundsQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<TeamLeagueRoundDto>> Handle(GetTeamLeagueRoundsQuery request, CancellationToken cancellationToken)
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
                teamLeague.Rounds = teamLeague.Rounds
                    .OrderBy(r => r.Name.Substring(r.Name.IndexOf(' ') + 1).PadLeft(2, '0'))
                    .ToList();
                teamLeague.Rounds.ForEach(r =>
                {
                    r.Matches = r.Matches.OrderBy(m => m.Id).ToList();
                    r.Matches.ForEach(m =>
                    {
                        m.MatchEntries = m.MatchEntries
                            .OrderByDescending(me => me.HomeAway.ToString())
                            .ToList();
                    });
                });
                
                return mapper.Map<IEnumerable<TeamLeagueRoundDto>>(teamLeague.Rounds);
            }

            return null;

        }
    }
}