using AutoMapper;
using LeagueManager.Application.Interfaces;
using LeagueManager.Application.Player.Dto;
using LeagueManager.Application.TeamLeagueMatches.Dto;
using L = LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class GetTeamLeagueMatchDetailsQueryHandler : IRequestHandler<GetTeamLeagueMatchDetailsQuery, TeamMatchDto>
    {
        private readonly ILeagueManagerDbContext context;
        private readonly IConfigurationProvider config;

        public GetTeamLeagueMatchDetailsQueryHandler(
            ILeagueManagerDbContext context,
            IConfigurationProvider config) => (this.context, this.config) = (context, config);

        public async Task<TeamMatchDto> Handle(GetTeamLeagueMatchDetailsQuery request, CancellationToken cancellationToken)
        {
            var mapper = config.CreateMapper();

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
                                TeamMatch = new TeamMatchDto { Guid = m.Guid },
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
                                    }).ToList(),
                                Goals = me.Goals.Select(g => 
                                    new Goals.GoalDto
                                    {
                                        Guid = g.Guid,
                                        TeamName = g.TeamMatchEntry.Team.Name,
                                        Minute = g.Minute,
                                        Player = mapper.Map<PlayerDto>(g.Player)
                                    }).ToList()
                            }).ToList()
                        }))
                )
                //.ProjectTo<TeamMatchDto>(config)
                .Where(m => m.Guid == request.Guid)
                .ToListAsync();

            if (matches.Count > 0)
                return matches[0];
            return null;
        }
    }
}