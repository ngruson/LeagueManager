using System;

namespace LeagueManager.Domain.Match
{
    public class TeamMatchEntrySubstitution
    {
        public int Id { get; set; }
        public virtual TeamMatchEntry TeamMatchEntry { get; set; }
        public Guid Guid { get; set; }
        public string Minute { get; set; }
        public virtual Player.Player PlayerOut { get; set; }
        public virtual Player.Player PlayerIn { get; set; }
    }
}