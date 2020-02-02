using System;

namespace LeagueManager.Application.TeamLeagueMatches.Lineup.Dto
{
    public class LineupEntryDto
    {
        public Guid Guid { get; set; }
        public string PlayerNumber { get; set; }
        public PlayerDto Player { get; set; }
        public string TeamName { get; set; }
    }
}