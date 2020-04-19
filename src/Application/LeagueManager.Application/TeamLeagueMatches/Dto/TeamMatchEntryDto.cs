using LeagueManager.Application.TeamLeagueMatches.Goals;
using LeagueManager.Application.TeamLeagueMatches.Lineup.Dto;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Dto
{
    public class TeamMatchEntryDto
    {
        public TeamMatchDto TeamMatch { get; set; }
        public TeamDto Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IntegerScoreDto Score { get; set; }
        public List<LineupEntryDto> Lineup { get; set; }
        public List<GoalDto> Goals { get; set; }
    }
}