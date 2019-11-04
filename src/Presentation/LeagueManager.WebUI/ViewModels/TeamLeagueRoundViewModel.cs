using System.Collections.Generic;

namespace LeagueManager.WebUI.ViewModels
{
    public class TeamLeagueRoundViewModel
    {
        public string Name { get; set; }
        public IEnumerable<TeamMatchViewModel> Matches { get; set; }
    }
}