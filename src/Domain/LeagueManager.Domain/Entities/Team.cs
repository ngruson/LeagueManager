namespace LeagueManager.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public byte[] Logo { get; set; }
    }
}