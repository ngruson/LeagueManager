using System;

namespace LeagueManager.Domain.Match
{
    public interface IMatch
    {
        DateTime? StartTime { get; set; }
    }
}