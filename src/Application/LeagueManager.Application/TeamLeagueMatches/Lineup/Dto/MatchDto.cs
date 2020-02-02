using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Lineup.Dto
{
    public class MatchDto
    {
        public Guid Guid { get; set; }
        public List<MatchEntryDto> MatchEntries { get; set; }
    }
}