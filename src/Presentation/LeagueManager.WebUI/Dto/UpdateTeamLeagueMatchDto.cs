using System;

namespace LeagueManager.WebUI.Dto
{
    public class UpdateTeamLeagueMatchDto
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime? StartTime { get; set; }
    }
}