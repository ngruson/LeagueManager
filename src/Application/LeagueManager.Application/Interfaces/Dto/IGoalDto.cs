using System;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface IGoalDto : IMatchEventDto
    {
        new Guid Guid { get; set; }
        new string TeamMatchEntryTeamName { get; set; }
        string Minute { get; set; }
        IPlayerDto Player { get; set; }
    }
}