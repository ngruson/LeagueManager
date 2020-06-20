using LeagueManager.Application.Interfaces.Dto;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class MatchEventsDto
    {
        public List<IGoalDto> Goals { get; set; }
        public List<ISubstitutionDto> Substitutions { get; set; }
    }
}