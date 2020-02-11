using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using L = LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class GetTeamLeagueMatchDetailsQueryHandler : IRequestHandler<GetTeamLeagueMatchDetailsQuery, TeamMatchDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IMapper mapper;

        public GetTeamLeagueMatchDetailsQueryHandler(
            ILeagueManagerDbContext context,
            IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<TeamMatchDto> Handle(GetTeamLeagueMatchDetailsQuery request, CancellationToken cancellationToken)
        {
            var matches = await context.TeamLeagues
                .Where(t => t.Name == request.LeagueName)
                .SelectMany(t => t.Rounds.SelectMany(r => r.Matches
                        .Select(m => new TeamMatchDto
                        {
                            Guid = m.Guid,
                            TeamLeague = m.TeamLeagueRound.TeamLeague.Name,
                            TeamLeagueRound = m.TeamLeagueRound.Name,
                            StartTime = m.StartTime,
                            MatchEntries = m.MatchEntries.Select(me => new TeamMatchEntryDto
                            {
                                HomeAway = mapper.Map<HomeAway>(me.HomeAway),
                                Team = mapper.Map<TeamDto>(me.Team),
                                Score = mapper.Map<IntegerScoreDto>(me.Score),
                                Lineup = me.Lineup.Select(lp =>
                                    new L.LineupEntryDto
                                    {
                                        Guid = lp.Guid,
                                        PlayerNumber = lp.Number,
                                        Player = mapper.Map<L.PlayerDto>(lp.Player),
                                        TeamName = me.Team.Name
                                    }).ToList()
                            }).ToList()
                        }))
                )
                .Where(m => m.Guid == request.Guid)
                .ToListAsync();

            if (matches.Count > 0)
                return matches[0];
            return null;
        }
    }
}