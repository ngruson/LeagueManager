using LeagueManager.Application.TeamLeagueMatches.Dto;
using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.WebUI.ViewModels
{
    public class TeamMatchEntryViewModel
    {
        public TeamMatchViewModel TeamMatch { get; set; }
        public TeamViewModel Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IntegerScoreViewModel Score { get; set; }
        public List<TeamMatchEntryLineupEntryViewModel> Lineup { get; set; }
        public List<TeamMatchEntryGoalEventViewModel> Goals { get; set; }
        public List<IMatchEventViewModel> Events
        {
            get
            {
                return Goals.ToList<IMatchEventViewModel>();
            }
        }
    }
}