using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class MatchEventsVm
    {
        public IList<GoalVm> Goals { get; set; }
    }
}