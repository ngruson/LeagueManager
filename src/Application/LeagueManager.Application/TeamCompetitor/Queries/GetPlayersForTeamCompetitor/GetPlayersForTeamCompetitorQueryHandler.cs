using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor
{
    public class GetPlayersForTeamCompetitorQueryHandler : IRequestHandler<GetPlayersForTeamCompetitorQuery, IEnumerable<CompetitorPlayerDto>>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;

        public GetPlayersForTeamCompetitorQueryHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config)
            => (this.context, this.config) = (context, config);
        
        public async Task<IEnumerable<CompetitorPlayerDto>> Handle(GetPlayersForTeamCompetitorQuery request, CancellationToken cancellationToken)
        {
            var teamLeague = await context.TeamLeagues
                .ProjectTo<TeamLeagueDto>(config)
                .SingleOrDefaultAsync(l => l.Name == request.LeagueName);

            if (teamLeague == null)
                throw new TeamLeagueNotFoundException(request.LeagueName);

            var team = teamLeague.Competitors
                .SingleOrDefault(t => t.TeamName == request.TeamName);
            if (team == null)
                throw new TeamNotFoundException(request.TeamName);

            return team.Players;
        }
    }
}