using System.Collections.Generic;

namespace LeagueManager.Domain.Match
{
    public interface ITeamMatch : IMatch
    {
        List<TeamMatchEntry> MatchEntries { get; set; }
    }
}