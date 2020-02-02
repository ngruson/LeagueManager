using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Lineup.Dto
{
    public class TeamLeagueDto
    {
        public string LeagueName { get; set; }
        public List<RoundDto> Rounds { get; set; }
    }
}