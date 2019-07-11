using LeagueManager.Domain.Entities;
using System.Collections.Generic;

namespace LeagueManager.Domain.Interfaces
{
    public interface ITeamLeague : ITeamCompetition
    {
        List<LeagueTeam> Teams { get; set; }
        List<LeagueRound> Rounds { get; set; }
    }
}