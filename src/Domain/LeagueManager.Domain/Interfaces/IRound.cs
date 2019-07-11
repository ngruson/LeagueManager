using LeagueManager.Domain.Entities;
using System.Collections.Generic;

namespace LeagueManager.Domain.Interfaces
{
    public interface IRound
    {
        string Name { get; set; }
        List<Match> Matches { get; set; }
    }
}