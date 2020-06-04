using AutoMapper;
using AutoMapper.QueryableExtensions;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class GetPlayerForTeamCompetitorQueryHandler : IRequestHandler<GetPlayerForTeamCompetitorQuery, CompetitorPlayerDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;

        public GetPlayerForTeamCompetitorQueryHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config) => (this.context, this.config) = (context, config);
        
        public async Task<CompetitorPlayerDto> Handle(GetPlayerForTeamCompetitorQuery request, CancellationToken cancellationToken)
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

            var player = team.Players.SingleOrDefault(p => p.Player.FullName == request.PlayerName);
            if (player == null)
                throw new PlayerNotFoundException(request.PlayerName);

            return player;
        }
    }
}