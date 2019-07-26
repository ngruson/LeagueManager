using AutoMapper;
using LeagueManager.Application.Competitions.Queries.Dto;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Competitions.Queries.GetTeamLeague
{
    public class GetTeamLeagueQueryHandler : IRequestHandler<GetTeamLeagueQuery, TeamLeagueDto>
    {
        private ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetTeamLeagueQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TeamLeagueDto> Handle(GetTeamLeagueQuery request, CancellationToken cancellationToken)
        {
            var teamLeague = await context.TeamLeagues
                .Include(t => t.Country)
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team)
                .Include(t => t.Rounds)
                    .ThenInclude(r => r.Matches)
                .SingleOrDefaultAsync(l => l.Name == request.Name, cancellationToken);

            if (teamLeague != null)
            {
                teamLeague.CalculateTable();
                return mapper.Map<TeamLeagueDto>(teamLeague);
            }

            return null;
        }
    }
}