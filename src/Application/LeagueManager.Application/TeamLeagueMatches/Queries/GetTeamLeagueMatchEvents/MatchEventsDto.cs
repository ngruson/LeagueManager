using LeagueManager.Application.TeamLeagueMatches.Goals;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class MatchEventsDto
    {
        public List<GoalDto> Goals { get; set; }
    }
}