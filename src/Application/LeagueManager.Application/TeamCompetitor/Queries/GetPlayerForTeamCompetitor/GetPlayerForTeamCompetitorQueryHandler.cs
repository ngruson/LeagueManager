using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamCompetitor.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayerForTeamCompetitor
{
    public class GetPlayerForTeamCompetitorQueryHandler : IRequestHandler<GetPlayerForTeamCompetitorQuery, TeamCompetitorPlayerDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetPlayerForTeamCompetitorQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<TeamCompetitorPlayerDto> Handle(GetPlayerForTeamCompetitorQuery request, CancellationToken cancellationToken)
        {
            var teamLeague = await context.TeamLeagues
                .Include(t => t.Competitors)
                    .ThenInclude(c => c.Team)
                .SingleOrDefaultAsync(t => t.Name == request.LeagueName);

            if (teamLeague == null)
                throw new TeamLeagueNotFoundException(request.LeagueName);

            var team = teamLeague.Competitors
                .SingleOrDefault(t => t.Team.Name == request.TeamName);
            if (team == null)
                throw new TeamNotFoundException(request.TeamName);

            var player = await context.TeamLeagues
                .Where(t => t.Name == teamLeague.Name)
                .SelectMany(t => t.Competitors.Where(c => c.Team.Name == request.TeamName))
                .SelectMany(x => x.Players.Select(p => new TeamCompetitorPlayerDto
                {
                    Number = p.Number,
                    Player = mapper.Map<PlayerDto>(p.Player)
                }))
                .SingleOrDefaultAsync(x => x.Player.FullName == request.PlayerName);

            if (player == null)
                throw new PlayerNotFoundException(request.PlayerName);

            return player;
        }
    }
}