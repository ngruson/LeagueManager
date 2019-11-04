namespace LeagueManager.Domain.Competitor
{
    public class TeamCompetitor : ITeamCompetitor
    {
        public int Id { get; set; }
        public Team Team { get; set; }
    }
}