using System;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface IMatchEventDto
    {
        Guid Guid { get; set; }
        string TeamMatchEntryTeamName { get; set; }
    }
}