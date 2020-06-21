using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Score;
using System;
using System.Collections.Generic;

namespace LeagueManager.Domain.Match
{
    public class TeamMatchEntry
    {
        public int Id { get; set; }
        public virtual TeamLeagueMatch TeamLeagueMatch { get; set; }
        public int? TeamId { get; set; }
        public virtual Team Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public virtual IntegerScore Score { get; set; }
        public virtual IEnumerable<TeamMatchEntryLineupEntry> Lineup { get; set; } = new List<TeamMatchEntryLineupEntry>();
        public virtual IEnumerable<TeamMatchEntryGoal> Goals { get; set; } = new List<TeamMatchEntryGoal>();
        public virtual IEnumerable<TeamMatchEntrySubstitution> Substitutions { get; set; } = new List<TeamMatchEntrySubstitution>();

        public void CreateLineup(int amountOfPlayers)
        {
            var list = new List<TeamMatchEntryLineupEntry>();
            for (int i = 0; i < amountOfPlayers; i++)
            {
                list.Add(new TeamMatchEntryLineupEntry { Guid = Guid.NewGuid() });
            }

            Lineup = list;
        }
    }
}