using System.Collections.Generic;

namespace LeagueManager.WebUI.ViewModels
{
    public class ViewTeamLeagueViewModel
    {
        public string Name { get; set; }
        public IEnumerable<TeamLeagueRoundViewModel> Rounds { get; set; }
        public string SelectedRound { get; set; }
        public TeamLeagueTableViewModel Table { get; set; }
    }
}