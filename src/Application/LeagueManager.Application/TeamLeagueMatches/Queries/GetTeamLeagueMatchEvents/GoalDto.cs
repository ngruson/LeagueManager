using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class GoalDto : IMapFrom<TeamMatchEntryGoal>, IGoalDto
    {
        public Guid Guid { get; set; }
        public string TeamMatchEntryTeamName { get; set; }
        public string Minute { get; set; }
        public IPlayerDto Player { get; set; }
    }
}