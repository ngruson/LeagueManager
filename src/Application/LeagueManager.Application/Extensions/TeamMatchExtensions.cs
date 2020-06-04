using System.Linq;

namespace LeagueManager.Application.Interfaces.Dto
{
    public static class TeamMatchExtensions
    {
        public static ITeamMatchEntryDto Home<T>(this ITeamMatchDto<T> match)
            where T : ITeamMatchEntryDto
        {
            return match.MatchEntries.SingleOrDefault(me => me.HomeAway == HomeAway.Home);
        }

        public static ITeamMatchEntryDto Away<T>(this ITeamMatchDto<T> match)
            where T : ITeamMatchEntryDto
        {
            return match.MatchEntries.SingleOrDefault(me => me.HomeAway == HomeAway.Away);
        }
    }
}