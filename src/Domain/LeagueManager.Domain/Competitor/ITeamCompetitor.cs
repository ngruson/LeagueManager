namespace LeagueManager.Domain.Competitor
{
    public interface ITeamCompetitor : ICompetitor
    {
        Team Team { get; set; }
    }
}