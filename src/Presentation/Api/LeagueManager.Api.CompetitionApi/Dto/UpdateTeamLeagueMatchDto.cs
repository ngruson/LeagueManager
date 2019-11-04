using System;

namespace LeagueManager.Api.CompetitionApi.Dto
{
    public class UpdateTeamLeagueMatchDto
    {
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public DateTime? StartTime { get; set; }
    }
}