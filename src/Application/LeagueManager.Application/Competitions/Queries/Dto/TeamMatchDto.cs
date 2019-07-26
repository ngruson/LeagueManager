using System;
using System.Collections.Generic;

namespace LeagueManager.Application.Competitions.Queries.Dto
{
    public class TeamMatchDto
    {
        public DateTime? StartTime { get; set; }
        public List<TeamMatchEntryDto> MatchEntries { get; set; }
    }
}