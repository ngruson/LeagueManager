using LeagueManager.Application.TeamLeagueMatches.Dto;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Dto
{
    public class TeamLeagueRoundDto
    {
        public string Name { get; set; }
        public string TeamLeague { get; set; }
        public IEnumerable<TeamMatchDto> Matches { get; set; }
    }
}