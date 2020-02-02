using Microsoft.EntityFrameworkCore;
using LeagueManager.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using LeagueManager.Domain.Competition;
using AutoMapper;
using LeagueManager.Application.Competitions.Queries.Dto;

namespace LeagueManager.Application.Competitions.Queries.GetCompetition
{
    public class GetCompetitionQueryHandler : IRequestHandler<GetCompetitionQuery, CompetitionDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetCompetitionQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<CompetitionDto> Handle(GetCompetitionQuery request, CancellationToken cancellationToken)
        {
            var list = new List<TeamLeague>();
            var teamLeague = await context.TeamLeagues
                .Include(t => t.Sports)
                .Include(t => t.Country)
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team)
                .SingleOrDefaultAsync(l => l.Name == request.Name, cancellationToken);

            if (teamLeague != null)
                list.Add(teamLeague);

            var competition = list.SingleOrDefault(c => c.Name == request.Name);

            if (competition != null)
                return mapper.Map<CompetitionDto>(competition);

            return null;
        }
    }
}