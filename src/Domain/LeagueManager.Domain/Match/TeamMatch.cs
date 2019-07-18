using System;
using System.Collections.Generic;

namespace LeagueManager.Domain.Match
{
    public class TeamMatch : ITeamMatch
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
        public List<TeamMatchEntry> MatchEntries { get; set; } = new List<TeamMatchEntry>();
    }
}