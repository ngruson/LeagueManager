namespace LeagueManager.Domain.Competitor
{
    public interface IPlayerCompetitor : ICompetitor
    {
        Domain.Player.Player Player { get; set; }
    }
}