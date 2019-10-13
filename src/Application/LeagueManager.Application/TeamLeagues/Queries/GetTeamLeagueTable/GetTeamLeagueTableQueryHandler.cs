using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.TeamLeagues.Queries.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable
{
    public class GetTeamLeagueTableQueryHandler : IRequestHandler<GetTeamLeagueTableQuery, TeamLeagueTableDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetTeamLeagueTableQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TeamLeagueTableDto> Handle(GetTeamLeagueTableQuery request, CancellationToken cancellationToken)
        {
            var teamLeague = await context.TeamLeagues
                .Include(t => t.Country)
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team)
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Matches)
                        .ThenInclude(m => m.MatchEntries)
                .SingleOrDefaultAsync(l => l.Name == request.LeagueName, cancellationToken);

            if (teamLeague == null)
                return null;

            teamLeague.CalculateTable();
            return mapper.Map<TeamLeagueTableDto>(teamLeague.Table);

        }
    }
}