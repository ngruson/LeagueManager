using LeagueManager.Domain.Competitor;
using System.Collections.Generic;

namespace LeagueManager.Domain.Competition
{
    public interface ICompetitionCompetitors<TCompetitor> where TCompetitor : ICompetitor
    {
        List<TCompetitor> Competitors { get; set; }
    }
}