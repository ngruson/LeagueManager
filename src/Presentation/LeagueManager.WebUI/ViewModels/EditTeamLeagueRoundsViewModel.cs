using LeagueManager.Domain.Competitor;
using System.Collections.Generic;

namespace LeagueManager.WebUI.ViewModels
{
    public class EditTeamLeagueRoundsViewModel
    {
        public string Name { get; set; }
        public IEnumerable<TeamViewModel> Teams { get; set; }
        public IEnumerable<TeamLeagueRoundViewModel> Rounds { get; set; }
        public string SelectedRound { get; set; }
    }
}