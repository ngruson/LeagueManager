using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry
{
    public class GetTeamLeagueMatchLineupEntryQueryHandler : IRequestHandler<GetTeamLeagueMatchLineupEntryQuery, LineupEntryDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;

        public GetTeamLeagueMatchLineupEntryQueryHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config) 
                => (this.context, this.config) = (context, config);
        
        public async Task<LineupEntryDto> Handle(GetTeamLeagueMatchLineupEntryQuery request, CancellationToken cancellationToken)
        {
            var lineupEntry = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(l => l.Rounds.SelectMany(r =>
                        r.Matches
                        .Where(m => m.Guid == request.MatchGuid)
                        .SelectMany(m => m.MatchEntries.SelectMany(me => me.Lineup))))
                .ProjectTo<LineupEntryDto>(config)
                .SingleOrDefaultAsync(l => l.Guid == request.LineupEntryGuid);

            if (lineupEntry == null)
                throw new LineupEntryNotFoundException(request.LineupEntryGuid);

            return lineupEntry;
        }
    }
}