using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Sports;

namespace LeagueManager.Domain.Competition
{
    public interface ITournament<TSports, TOptions, TCompetitor> : ICompetition, ICompetitionCompetitors<TCompetitor>, ICompetitionSports<TSports, TOptions>
        where TSports : ISports<TOptions>
        where TCompetitor : ICompetitor
    {
    }
}