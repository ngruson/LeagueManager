using System.Collections.Generic;

namespace LeagueManager.Domain.Entities
{
    public abstract class League : Competition
    {
        public List<LeagueRound> Rounds { get; set; } = new List<LeagueRound>();
    }
}