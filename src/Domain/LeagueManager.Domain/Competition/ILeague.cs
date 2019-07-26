using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Points;
using System.Collections.Generic;

namespace LeagueManager.Domain.Competition
{
    public interface ILeague<TCompetitor, TRound> : ICompetition<TCompetitor>
        where TCompetitor : ICompetitor
    {
        List<TRound> Rounds { get; set; }
        PointSystem PointSystem { get; set; }

        void CalculateTable();
    }
}