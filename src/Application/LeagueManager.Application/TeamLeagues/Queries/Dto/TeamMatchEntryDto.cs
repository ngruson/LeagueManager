namespace LeagueManager.Application.TeamLeagues.Queries.Dto
{
    public class TeamMatchEntryDto
    {
        public TeamDto Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IntegerScoreDto Score { get; set; }
    }
}