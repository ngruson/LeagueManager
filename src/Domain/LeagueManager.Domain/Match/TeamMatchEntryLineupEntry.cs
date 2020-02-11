using System;

namespace LeagueManager.Domain.Match
{
    public class TeamMatchEntryLineupEntry
    {
        public int Id { get; set; }
        public TeamMatchEntry TeamMatchEntry { get; set; }
        public Guid Guid { get; set; }
        public string Number { get; set; }
        public Player.Player Player { get; set; }
    }
}