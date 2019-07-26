using LeagueManager.Domain.Common;
using LeagueManager.Domain.Competitor;
using System.Collections.Generic;

namespace LeagueManager.Domain.Competition
{
    public interface ICompetition<TCompetitor>
        where TCompetitor : ICompetitor
    {        
        string Name { get; set; }
        Country Country { get; set; }
        byte[] Logo { get; set; }

        List<TCompetitor> Competitors { get; set; }
    }
}