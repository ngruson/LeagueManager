using Microsoft.EntityFrameworkCore;
using LeagueManager.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using LeagueManager.Domain.Competition;

namespace LeagueManager.Application.Competitions.Queries.GetCompetition
{
    public class GetCompetitionQueryHandler : IRequestHandler<GetCompetitionQuery, CompetitionDto>
    {
        private readonly ILeagueManagerDbContext context;

        public GetCompetitionQueryHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<CompetitionDto> Handle(GetCompetitionQuery request, CancellationToken cancellationToken)
        {
            var list = new List<ICompetition>();
            var teamLeague = await context.TeamLeagues.SingleOrDefaultAsync(l => l.Name == request.Name, cancellationToken);
            if (teamLeague != null)
                list.Add(teamLeague);

            var competition = list.SingleOrDefault(c => c.Name == request.Name);

            if (competition != null)
                return new CompetitionDto {
                    Name = competition.Name,
                    Country = competition.Country?.Name,
                    Logo = competition.Logo
                };

            return null;
        }
    }
}