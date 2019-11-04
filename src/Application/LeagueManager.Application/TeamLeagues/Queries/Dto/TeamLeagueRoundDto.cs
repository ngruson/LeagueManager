using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagues.Queries.Dto
{
    public class TeamLeagueRoundDto
    {
        public string Name { get; set; }
        public IEnumerable<TeamMatchDto> Matches { get; set; }
    }
}