using LeagueManager.Application.Player.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Goals
{
    public class GoalDto
    {
        public Guid Guid { get; set; }
        public string TeamName { get; set; }
        public string Minute { get; set; }
        public PlayerDto Player { get; set; }
    }
}