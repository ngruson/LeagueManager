using LeagueManager.Domain.Entities;
using System.Collections.Generic;

namespace LeagueManager.Domain.Interfaces
{
    public interface ICompetition
    {
        List<LeagueRound> Rounds { get; set; }
        void GenerateSchedule();
    }
}