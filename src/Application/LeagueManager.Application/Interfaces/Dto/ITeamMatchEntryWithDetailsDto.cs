using System;
using System.Collections.Generic;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ITeamMatchEntryWithDetailsDto : ITeamMatchEntryDto
    {
        new Guid TeamLeagueMatchGuid { get; set; }
        new ITeamDto Team { get; set; }
        new HomeAway HomeAway { get; set; }
        new IIntegerScoreDto Score { get; set; }
        public List<ILineupEntryDto> Lineup { get; set; }
        public List<IGoalDto> Goals { get; set; }
    }
}