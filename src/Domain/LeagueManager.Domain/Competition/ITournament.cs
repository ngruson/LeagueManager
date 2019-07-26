using LeagueManager.Domain.Competitor;

namespace LeagueManager.Domain.Competition
{
    public interface ITournament<TCompetitor> : ICompetition<TCompetitor>
        where TCompetitor : ICompetitor
    {
    }
}