using LeagueManager.Domain.Common;

namespace LeagueManager.Domain.Competitor
{
    public class Team : ITeam
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
        public byte[] Logo { get; set; }
    }
}