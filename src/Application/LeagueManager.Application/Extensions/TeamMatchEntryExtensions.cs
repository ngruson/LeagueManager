using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.Application.Interfaces.Dto
{
    public static class TeamMatchEntryExtensions
    {
        public static List<IMatchEventDto> Events(this ITeamMatchEntryWithDetailsDto matchEntry)
        {
            var list = new List<IMatchEventDto>();
            if (matchEntry.Goals != null)
                list.AddRange(matchEntry.Goals);
            if (matchEntry.Substitutions != null)
                list.AddRange(matchEntry.Substitutions);

            list = list.OrderBy(x => int.Parse(x.Minute)).ToList();

            return list;
        }
    }
}