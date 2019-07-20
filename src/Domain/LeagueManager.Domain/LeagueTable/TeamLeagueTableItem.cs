using LeagueManager.Domain.Competitor;

namespace LeagueManager.Domain.LeagueTable
{
    public class TeamLeagueTableItem : ITeamLeagueTableItem
    {
        public int Position { get; set; }
        public Team Team { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesDrawed { get; set; }
        public int GamesLost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference
        {
            get
            {
                return GoalsFor - GoalsAgainst;
            }
        }
        public int Points { get; set; }
    }
}