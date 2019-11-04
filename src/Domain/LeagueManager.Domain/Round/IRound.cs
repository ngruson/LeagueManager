using System.Collections.Generic;

namespace LeagueManager.Domain.Round
{
    public interface IRound
    {
        int Id { get; set; }
        string Name { get; set; }
    }
}