using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.Dto
{
    public class TeamMatchDto
    {
        public Guid Guid { get; set; }
        public DateTime? StartTime { get; set; }
        public List<TeamMatchEntryDto> MatchEntries { get; set; }
    }
}