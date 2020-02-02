using System.Collections.Generic;

namespace LeagueManager.Domain.Competitor
{
    public class TeamCompetitor : ITeamCompetitor
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public List<TeamCompetitorPlayer> Players { get; set; } = new List<TeamCompetitorPlayer>();
    }
}