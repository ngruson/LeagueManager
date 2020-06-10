using System;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ITeamMatchEntryDto
    {
        Guid TeamLeagueMatchGuid { get; set; }
        ITeamDto Team { get; set; }
        HomeAway HomeAway { get; set; }
        IIntegerScoreDto Score { get; set; }
    }
}