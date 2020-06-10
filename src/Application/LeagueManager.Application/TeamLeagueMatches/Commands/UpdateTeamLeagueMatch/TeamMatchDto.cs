using LeagueManager.Application.Common.Mappings;
using LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using System;
using System.Collections.Generic;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch
{
    public class TeamMatchDto : IMapFrom<TeamLeagueMatch>, ITeamMatchDto<ITeamMatchEntryDto>
    {
        public Guid Guid { get; set; }
        public string TeamLeagueName { get; set; }
        public string RoundName { get; set; }
        public DateTime? StartTime { get; set; }
        public List<ITeamMatchEntryDto> MatchEntries { get; set; }
    }
}