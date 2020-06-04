using System;
using System.Linq;

namespace LeagueManager.Application.Interfaces.Dto
{
    public static class TeamLeagueExtensions
    {
        public static TMatch GetMatch<TRound, TMatch, TMatchEntry>(this ITeamLeagueDto<TRound> teamLeague, Guid guid) 
            where TRound : IRoundDto<TMatch>
            where TMatch : ITeamMatchDto<TMatchEntry>
            where TMatchEntry : ITeamMatchEntryDto
        {
            foreach (var round in teamLeague.Rounds.ToList())
            {
                foreach (var match in round.Matches)
                {
                    if (match.Guid == guid)
                        return match;
                }
            }
            return default;
        }
    }
}