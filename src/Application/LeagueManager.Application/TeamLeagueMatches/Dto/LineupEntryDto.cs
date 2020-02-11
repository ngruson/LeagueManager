using LeagueManager.Application.Player.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Dto
{
    public class LineupEntryDto_old
    {
        public string TeamName { get; set; }
        public Guid Guid { get; set; }
        public string PlayerNumber { get; set; }
        public PlayerDto Player { get; set; }
    }
}