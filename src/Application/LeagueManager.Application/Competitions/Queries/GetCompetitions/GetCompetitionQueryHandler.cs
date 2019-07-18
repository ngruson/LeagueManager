using LeagueManager.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using LeagueManager.Domain.Competition;
using Microsoft.EntityFrameworkCore;

namespace LeagueManager.Application.Competitions.Queries.GetCompetitions
{
    public class GetCompetitionsQueryHandler : IRequestHandler<GetCompetitionsQuery, IEnumerable<CompetitionDto>>
    {
        private readonly ILeagueManagerDbContext context;

        public GetCompetitionsQueryHandler(ILeagueManagerDbContext context)
        {
            this.context = context;
        }

        private async Task<IEnumerable<ICompetition>> GetTeamLeagues(string country)
        {
            IQueryable<TeamLeague> query = context.TeamLeagues;
            if (!string.IsNullOrEmpty(country))
                query = query.Where(l => l.Country != null && l.Country.Name == country);

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<CompetitionDto>> Handle(GetCompetitionsQuery request, CancellationToken cancellationToken)
        {
            var list = new List<ICompetition>();
            var teamLeagues = await GetTeamLeagues(request.Country);
            if (teamLeagues != null)
                list.AddRange(teamLeagues);

            return list.Select(c => new CompetitionDto
            {
                Name = c.Name,
                Country = c.Country?.Name,
                Logo = c.Logo
            });
        }
    }
}