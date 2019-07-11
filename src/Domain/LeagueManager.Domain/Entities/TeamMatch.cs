using System.Collections.Generic;

namespace LeagueManager.Domain.Entities
{
    public class TeamMatch : Match
    {
        public List<TeamMatchEntry> MatchEntries { get; set; }
    }
}