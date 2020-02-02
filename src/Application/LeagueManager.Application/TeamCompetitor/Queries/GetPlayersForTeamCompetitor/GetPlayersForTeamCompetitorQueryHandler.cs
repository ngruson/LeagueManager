using AutoMapper;
using LeagueManager.Application.Exceptions;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamCompetitor.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamCompetitor.Queries.GetPlayersForTeamCompetitor
{
    public class GetPlayersForTeamCompetitorQueryHandler : IRequestHandler<GetPlayersForTeamCompetitorQuery, IEnumerable<TeamCompetitorPlayerDto>>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetPlayersForTeamCompetitorQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }
        public async Task<IEnumerable<TeamCompetitorPlayerDto>> Handle(GetPlayersForTeamCompetitorQuery request, CancellationToken cancellationToken)
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

            var players = await context.TeamLeagues
                .Where(t => t.Name == teamLeague.Name)
                .SelectMany(t => t.Competitors.Where(c => c.Team.Name == request.TeamName))
                .SelectMany(x => x.Players
                    .OrderBy(p => p.Player.LastName).ThenBy(p => p.Player.FirstName)
                    .Select(p => new TeamCompetitorPlayerDto
                    {
                        Number = p.Number,
                        Player = mapper.Map<PlayerDto>(p.Player)
                    })
                )
                .ToListAsync();

            return players;
        }
    }
}