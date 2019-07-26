namespace LeagueManager.Application.Competitions.Queries.Dto
{
    public class TeamMatchEntryDto
    {
        public string Team { get; set; }
        public string HomeAway { get; set; }
        public IntegerScoreDto Score { get; set; }
    }
}