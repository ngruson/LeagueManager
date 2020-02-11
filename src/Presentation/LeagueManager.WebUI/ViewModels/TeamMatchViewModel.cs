using LeagueManager.Application.TeamLeagueMatches.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LeagueManager.WebUI.ViewModels
{
    public class TeamMatchViewModel
    {
        public string TeamLeague { get; set; }
        public Guid Guid { get; set; }
        public DateTime? StartTime { get; set; }
        public List<TeamMatchEntryViewModel> MatchEntries { get; set; }

        public TeamMatchEntryViewModel Home
        {
            get
            {
                return MatchEntries.SingleOrDefault(m => m.HomeAway == HomeAway.Home);
            }
        }

        public TeamMatchEntryViewModel Away
        {
            get
            {
                return MatchEntries.SingleOrDefault(m => m.HomeAway == HomeAway.Away);
            }
        }
    }
}