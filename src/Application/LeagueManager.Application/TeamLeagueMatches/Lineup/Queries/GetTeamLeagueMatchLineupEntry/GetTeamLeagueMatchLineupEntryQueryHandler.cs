using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry
{
    public class GetTeamLeagueMatchLineupEntryQueryHandler : IRequestHandler<GetTeamLeagueMatchLineupEntryQuery, LineupEntryDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;
        
        public GetTeamLeagueMatchLineupEntryQueryHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config)
        {
            this.context = context;
            this.config = config;
        }
        public async Task<LineupEntryDto> Handle(GetTeamLeagueMatchLineupEntryQuery request, CancellationToken cancellationToken)
        {
            var lineupEntry = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .Select(l => l.Rounds.SelectMany(r =>
                        r.Matches.Where(m => m.Guid == request.MatchGuid)
                    ).FirstOrDefault())
                .SelectMany(m => m.MatchEntries.SelectMany(me => me.Lineup))
                .ProjectTo<LineupEntryDto>(config)
                .SingleOrDefaultAsync(l => l.Guid == request.LineupEntryGuid);

            if (lineupEntry == null)
                throw new LineupEntryNotFoundException(request.LineupEntryGuid);

            return lineupEntry;
        }
    }
}