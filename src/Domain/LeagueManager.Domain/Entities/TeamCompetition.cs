using System.Collections.Generic;

namespace LeagueManager.Domain.Entities
{
    public class TeamCompetition : Competition
    {
        public List<CompetingTeam> Teams { get; set; } = new List<CompetingTeam>();        
    }
}