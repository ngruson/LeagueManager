using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class GoalVm : IMapFrom<TeamMatchEntryGoal>
    {
        public Guid Guid { get; set; }
        public string TeamName { get; set; }
        public string Minute { get; set; }
        public PlayerDto Player { get; set; }
    }
}