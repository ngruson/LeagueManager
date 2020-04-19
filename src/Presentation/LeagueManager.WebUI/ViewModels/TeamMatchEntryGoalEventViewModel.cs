using System;

namespace LeagueManager.WebUI.ViewModels
{
    public class TeamMatchEntryGoalEventViewModel : IMatchEventViewModel
    {
        public TeamMatchEntryGoalEventViewModel() => ViewName = "ViewGoalMatchEvent";
        
        public Guid Guid { get; set; }
        public string ViewName { get; }
        public string TeamName { get; set; }
        public string Minute { get; set; }
        public PlayerViewModel Player { get; set; }
    }
}