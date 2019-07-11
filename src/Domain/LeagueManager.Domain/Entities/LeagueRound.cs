using LeagueManager.Domain.Interfaces;
using System.Collections.Generic;

namespace LeagueManager.Domain.Entities
{
    public class LeagueRound : IRound
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Match> Matches { get; set; } = new List<Match>();
    }
}