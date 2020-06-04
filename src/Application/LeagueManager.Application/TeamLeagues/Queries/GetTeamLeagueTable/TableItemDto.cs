using LeagueManager.Application.Common.Mappings;
using LeagueManager.Domain.LeagueTable;

namespace LeagueManager.Application.TeamLeagues.Queries.GetTeamLeagueTable
{
    public class TableItemDto : IMapFrom<TeamLeagueTableItem>
    {
        public int Position { get; set; }
        public TeamDto Team { get; set; }
        public int GamesPlayed { get; set; }
        public int GamesWon { get; set; }
        public int GamesDrawn { get; set; }
        public int GamesLost { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int Points { get; set; }
    }
}