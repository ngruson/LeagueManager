using LeagueManager.Application.Interfaces.Dto;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchEvents
{
    public class TeamMatchDto : ITeamMatchDto<ITeamMatchEntryEventsDto>
    {
        public Guid Guid { get; set; }
        public string TeamLeagueName { get; set; }
        public string RoundName { get; set; }
        public DateTime? StartTime { get; set; }
        public List<ITeamMatchEntryEventsDto> MatchEntries { get; set; } = new List<ITeamMatchEntryEventsDto>();
    }
}