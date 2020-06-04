namespace LeagueManager.Domain.Sports
{
    public class TeamSports : ISports<TeamSportsOptions>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual TeamSportsOptions Options { get; set; }
    }
}