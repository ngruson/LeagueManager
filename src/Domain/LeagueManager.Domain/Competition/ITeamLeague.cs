using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Round;
using System.Collections.Generic;

namespace LeagueManager.Domain.Competition
{
    public interface ITeamLeague : ILeague
    {
        List<TeamCompetitor> Teams { get; set; }
        List<TeamLeagueRound> Rounds { get; set; }
    }
}