using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Dto
{
    public class TeamMatchDto
    {
        public Guid Guid { get; set; }
        public string TeamLeague { get; set; }
        public string TeamLeagueRound { get; set; }
        public DateTime? StartTime { get; set; }
        public List<TeamMatchEntryDto> MatchEntries { get; set; }
    }
}