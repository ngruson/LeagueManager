using System;

namespace LeagueManager.WebUI.ViewModels
{
    public class TeamMatchEntryLineupEntryViewModel
    {
        public Guid Guid { get; set; }
        public string TeamName { get; set; }
        public string Number { get; set; }
        public PlayerViewModel Player { get; set; }
    }
}