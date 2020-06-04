using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.Application.Interfaces.Dto
{
    public static class TeamMatchEntryExtensions
    {
        public static List<IMatchEventDto> Events(this ITeamMatchEntryWithDetailsDto matchEntry)
        {
            if (matchEntry.Goals != null)
                return matchEntry.Goals.ToList<IMatchEventDto>();
            return new List<IMatchEventDto>();
        }
    }
}