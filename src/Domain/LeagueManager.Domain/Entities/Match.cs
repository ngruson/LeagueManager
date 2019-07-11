using System;

namespace LeagueManager.Domain.Entities
{
    public abstract class Match
    {
        public int Id { get; set; }
        public DateTime? StartTime { get; set; }
    }
}