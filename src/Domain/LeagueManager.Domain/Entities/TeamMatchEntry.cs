namespace LeagueManager.Domain.Entities
{
    public class TeamMatchEntry
    {
        public int Id { get; set; }
        public Team Team { get; set; }
        public Score Score { get; set; }
    }
}