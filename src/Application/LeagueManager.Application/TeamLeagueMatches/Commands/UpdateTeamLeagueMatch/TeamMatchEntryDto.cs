using LeagueManager.Application.Common.Mappings;
using Dto = LeagueManager.Application.Interfaces.Dto;
using LeagueManager.Domain.Match;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Commands.UpdateTeamLeagueMatch
{
    public class TeamMatchEntryDto : IMapFrom<TeamMatchEntry>, Dto.ITeamMatchEntryDto
    {
        public Guid TeamLeagueMatchGuid { get; set; }
        public Dto.ITeamDto Team { get; set; }
        public Dto.HomeAway HomeAway { get; set; }
        public Dto.IIntegerScoreDto Score { get; set; }
    }
}