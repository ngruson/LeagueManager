using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Lineup.Queries.GetTeamLeagueMatchLineupEntry
{
    public class GetTeamLeagueMatchLineupEntryQuery : IRequest<LineupEntryDto>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public Guid LineupEntryGuid { get; set; }
    }
}