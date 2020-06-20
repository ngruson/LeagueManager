using System;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ISubstitutionDto : IMatchEventDto
    {
        new Guid Guid { get; set; }
        new string TeamMatchEntryTeamName { get; set; }
        new string Minute { get; set; }
        IPlayerDto PlayerIn { get; set; }
        IPlayerDto PlayerOut { get; set; }
    }
}