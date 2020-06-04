using System.Collections.Generic;

namespace LeagueManager.Domain.Competitor
{
    public class TeamCompetitor : ITeamCompetitor
    {
        public int Id { get; set; }
        public virtual Team Team { get; set; }
        public virtual List<TeamCompetitorPlayer> Players { get; set; } = new List<TeamCompetitorPlayer>();
    }
}