using System.Collections.Generic;

namespace LeagueManager.Domain.Entities
{
    public class TeamLeague : League
    {
        public List<CompetingTeam> Teams { get; set; } = new List<CompetingTeam>();
    }
}