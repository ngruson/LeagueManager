using System;
using System.Collections.Generic;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ITeamMatchDto<T>
    {
        string TeamLeagueName { get; set; }
        string RoundName { get; set; }
        Guid Guid { get; set; }
        DateTime? StartTime { get; set; }
        List<T> MatchEntries { get; set; }
    }
}