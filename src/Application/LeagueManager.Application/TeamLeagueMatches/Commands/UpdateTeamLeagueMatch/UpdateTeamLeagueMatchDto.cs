using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch
{
    public class UpdateTeamLeagueMatchDto
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime? StartTime { get; set; }
    }
}