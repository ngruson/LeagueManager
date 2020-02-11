using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Dto
{
    public class TeamMatchEntryDto
    {
        public TeamDto Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IntegerScoreDto Score { get; set; }
        public List<LineupEntryDto> Lineup { get; set; }
    }
}