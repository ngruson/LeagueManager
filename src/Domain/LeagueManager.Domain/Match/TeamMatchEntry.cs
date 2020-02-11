using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Score;
using System;
using System.Collections.Generic;

namespace LeagueManager.Domain.Match
{
    public class TeamMatchEntry
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public Team Team { get; set; }
        public HomeAway HomeAway { get; set; }
        public IntegerScore Score { get; set; }
        public IEnumerable<TeamMatchEntryLineupEntry> Lineup { get; set; } = new List<TeamMatchEntryLineupEntry>();

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