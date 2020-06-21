using System.Collections.Generic;

namespace LeagueManager.Application.Interfaces.Dto
{
    public interface ITeamMatchEntryEventsDto : ITeamMatchEntryDto
    {
        public List<IGoalDto> Goals { get; set; }
        public List<ISubstitutionDto> Substitutions { get; set; }
    }
}