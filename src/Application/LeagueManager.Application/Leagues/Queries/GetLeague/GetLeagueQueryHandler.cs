using Microsoft.EntityFrameworkCore;
using LeagueManager.Application.Interfaces;
using LeagueManager.Domain.Entities;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.Leagues.Queries.GetLeague
{
    public class GetLeagueQueryHandler : IRequestHandler<GetLeagueQuery,LeagueDto>
    {
        private readonly ILeagueManagerDbContext context;

        public GetLeagueQueryHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        public async Task<LeagueDto> Handle(GetLeagueQuery request, CancellationToken cancellationToken)
        {
            var competition = await context.Competitions.SingleOrDefaultAsync(l => l.Name == request.Name, cancellationToken);

            if (competition != null)
                return new LeagueDto { Name = competition.Name };

            return null;
        }
    }
}