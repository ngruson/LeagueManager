namespace LeagueManager.Domain.Competitor
{
    public class TeamCompetitorPlayer
    {
        public int Id { get; set; }
        public string Number { get; set; } 
        public virtual Player.Player Player { get; set; }
    }
}