namespace LeagueManager.Domain.LeagueTable
{
    public interface ILeagueTableItem
    {
        int Position { get; set; }
        int GamesPlayed { get; set; }
        int GamesWon { get; set; }
        int GamesDrawed { get; set; }
        int GamesLost { get; set; }
        int GoalsFor { get; set; }
        int GoalsAgainst { get; set; }
        int GoalDifference { get; }
        int Points { get; set; }
    }
}