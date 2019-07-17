namespace LeagueManager.Domain.Entities
{
    public abstract class Competition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Logo { get; set; }
    }
}