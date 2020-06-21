using System;
using System.Collections.Generic;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ITeamMatchEntryWithDetailsDto : ITeamMatchEntryEventsDto
    {
        new Guid TeamLeagueMatchGuid { get; set; }
        new ITeamDto Team { get; set; }
        new HomeAway HomeAway { get; set; }
        new IIntegerScoreDto Score { get; set; }
        public List<ILineupEntryDto> Lineup { get; set; }
        new public List<IGoalDto> Goals { get; set; }
        new public List<ISubstitutionDto> Substitutions { get; set; }
    }
}