using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Points;
using LeagueManager.Domain.Sports;
using System.Collections.Generic;

namespace LeagueManager.Domain.Competition
{
    public interface ILeague<TSports, TOptions, TCompetitor, TRound> : ICompetition, ICompetitionCompetitors<TCompetitor>, ICompetitionSports<TSports, TOptions>
        where TSports : ISports<TOptions>
        where TCompetitor : ICompetitor
    {
        List<TRound> Rounds { get; set; }
        PointSystem PointSystem { get; set; }

        void CalculateTable();
    }
}