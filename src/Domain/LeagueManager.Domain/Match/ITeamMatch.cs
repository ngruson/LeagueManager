using System.Collections.Generic;

namespace LeagueManager.Domain.Match
{
    public interface ITeamMatch : IMatch
    {
        IEnumerable<TeamMatchEntry> MatchEntries { get; set; }
    }
}