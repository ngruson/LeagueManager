using LeagueManager.Application.Interfaces.Dto;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamMatchDto : ITeamMatchDto<ITeamMatchEntryWithDetailsDto>
    {
        public Guid Guid { get; set; }
        public string TeamLeagueName { get; set; }
        public string RoundName { get; set; }
        public DateTime? StartTime { get; set; }
        public List<ITeamMatchEntryWithDetailsDto> MatchEntries { get; set; } = new List<ITeamMatchEntryWithDetailsDto>();
    }
}