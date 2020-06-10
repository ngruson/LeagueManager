using System.Collections.Generic;
using LeagueManager.Application.Interfaces.Dto;
using System;

namespace LeagueManager.Application.TeamLeagueMatches.Queries.GetTeamLeagueMatchDetails
{
    public class TeamMatchEntryDto : ITeamMatchEntryWithDetailsDto
    {
        public Guid TeamLeagueMatchGuid { get; set; }
        public ITeamDto Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IIntegerScoreDto Score { get; set; }
        public List<ILineupEntryDto> Lineup { get; set; }
        public List<IGoalDto> Goals { get; set; }
    }
}