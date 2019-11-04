using LeagueManager.Application.TeamLeagues.Queries.Dto;

namespace LeagueManager.WebUI.ViewModels
{
    public class TeamMatchEntryViewModel
    {
        public TeamViewModel Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IntegerScoreViewModel Score { get; set; }
    }
}