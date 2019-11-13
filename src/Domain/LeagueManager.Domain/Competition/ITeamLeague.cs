using LeagueManager.Domain.Competitor;
using LeagueManager.Domain.Round;

namespace LeagueManager.Domain.Competition
{
    public interface ITeamLeague : ILeague<TeamCompetitor, TeamLeagueRound>
    {
    }
}