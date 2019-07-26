using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Points;
using LeagueManager.Domain.Round;
using System.Collections.Generic;

namespace LeagueManager.Domain.LeagueTable
{
    public interface ILeagueTable
    {
        void CalculateTable(
            List<TeamCompetitor> teams,
            List<TeamLeagueRound> rounds,
            PointSystem pointSystem);
    }
}