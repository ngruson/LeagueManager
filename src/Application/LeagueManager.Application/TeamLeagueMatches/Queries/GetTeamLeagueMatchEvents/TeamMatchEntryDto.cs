using System.Collections.Generic;
using LeagueManager.Application.Interfaces.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class TeamMatchEntryDto : ITeamMatchEntryEventsDto
    {
        public Guid TeamLeagueMatchGuid { get; set; }
        public ITeamDto Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IIntegerScoreDto Score { get; set; }
        public List<IGoalDto> Goals { get; set; }
        public List<ISubstitutionDto> Substitutions { get; set; }
    }
}