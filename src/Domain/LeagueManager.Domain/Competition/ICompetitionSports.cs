using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Sports;

namespace LeagueManager.Domain.Competition
{
    public interface ICompetitionSports<TSports, TOptions>
        where TSports : ISports<TOptions>
    {
        TSports Sports { get; set; }
    }
}