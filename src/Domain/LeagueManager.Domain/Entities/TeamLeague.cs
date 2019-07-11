using LeagueManager.Domain.Interfaces;
using System.Collections.Generic;

namespace LeagueManager.Domain.Entities
{
    public class TeamLeague : ITeamLeague
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LeagueTeam> Teams { get; set;  } = new List<LeagueTeam>();
        public List<LeagueRound> Rounds { get; set; } = new List<LeagueRound>();
    }
}