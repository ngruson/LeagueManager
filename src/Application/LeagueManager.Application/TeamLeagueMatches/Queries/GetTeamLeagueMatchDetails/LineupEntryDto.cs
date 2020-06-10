using LeagueManager.Application.Interfaces.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class LineupEntryDto : ILineupEntryDto
    {
        public Guid Guid { get; set; }
        public string PlayerNumber { get; set; }
        public IPlayerDto Player { get; set; }
        public string TeamMatchEntryTeamName { get; set; }
    }
}