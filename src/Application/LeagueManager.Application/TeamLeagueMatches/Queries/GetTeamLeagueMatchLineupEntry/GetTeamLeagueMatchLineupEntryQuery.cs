using MediatR;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchLineupEntry
{
    public class GetTeamLeagueMatchLineupEntryQuery : IRequest<LineupEntryDto>
    {
        public string LeagueName { get; set; }
        public Guid MatchGuid { get; set; }
        public Guid LineupEntryGuid { get; set; }
    }
}