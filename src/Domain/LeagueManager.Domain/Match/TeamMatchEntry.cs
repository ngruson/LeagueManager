using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Score;

namespace LeagueManager.Domain.Match
{
    public class TeamMatchEntry
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IntegerScore Score { get; set; }
    }
}