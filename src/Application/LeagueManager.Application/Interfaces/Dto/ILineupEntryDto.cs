using System;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ILineupEntryDto
    {
        Guid Guid { get; set; }
        public string PlayerNumber { get; set; }
        IPlayerDto Player { get; set; }
        string TeamMatchEntryTeamName { get; set; }
    }
}